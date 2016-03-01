using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ab4645_Breakout
{
    class Ball : GameObject
    {
        protected float radius;
        protected Player player;
        protected float maxY;
        protected float speed = 10.0f;
        protected ParticleEmitter trail;
        protected Vector2 oldLinearVelocity = Vector2.Zero;
        public bool justCollided = false;

        public Player Owner {
            get { return player; }
        }

        public float Radius { get { return radius; } }


        public Ball(Player player, Vector2 position, float radius, float maxY, World world) : base(world) {
            this.player = player;
            this.radius = ConvertUnits.ToSimUnits(player.BallRadius);
            this.maxY = maxY;

            switch (player.Index)
            {
                case PlayerIndex.One:
                    trail = ParticleEmitter.SpawnGenericProjectileEmitterRed(ConvertUnits.ToDisplayUnits(position));
                    break;
                case PlayerIndex.Two:
                    trail = ParticleEmitter.SpawnGenericProjectileEmitterBlue(ConvertUnits.ToDisplayUnits(position));
                    break;
                default:
                    trail = ParticleEmitter.SpawnGenericProjectileEmitterRed(ConvertUnits.ToDisplayUnits(position));
                    break;
            }
            

            body = BodyFactory.CreateCircle(world, radius, 1, position);
            body.BodyType = BodyType.Dynamic;
            body.CollidesWith = Category.Cat2 | Category.Cat4;
            body.CollisionCategories = Category.Cat1;
            body.Restitution = 1.0f;
            body.Friction = 0.0f;
            body.OnCollision += OnCollision;

            //body.ApplyLinearImpulse(new Vector2(-0.2f, -1f)*0.8f);
            body.UserData = this;

            speed = Owner.BallSpeed;
            //body.LinearVelocity = Vector2.Normalize(new Vector2(-1f, -1f))*10f;
        }

        public bool OnCollision(Fixture a, Fixture b, Contact c)
        {

            if (a.Body.UserData == this)
            {
                if (!(b.Body.UserData is Paddle) && !(b.Body.UserData is Ball))
                    justCollided = true;
                if(b.Body.UserData is Paddle)
                {
                    Paddle p = b.Body.UserData as Paddle;

                    if (p.Sticky)
                    {
                        p.Attach(this);
                    } else
                    {
                        float shuntAmount = MathHelper.Lerp(-3 * MathHelper.PiOver4, -MathHelper.PiOver4, (0.5f + ((a.Body.UserData as Ball).Position.X - p.Body.Position.X) / p.width));
                        Vector2 shuntVector = new Vector2((float)Math.Cos(shuntAmount), (float)Math.Sin(shuntAmount));

                        float shuntStrength = 0.002f;
                        body.ApplyLinearImpulse(shuntVector * shuntStrength);
                    }
                    
                }
            }
            else if (b.Body.UserData == this)
            {
                if (!(a.Body.UserData is Paddle) && !(a.Body.UserData is Ball))
                    justCollided = true;
                if (a.Body.UserData is Paddle)
                {
                    Paddle p = a.Body.UserData as Paddle;
                    if (p.Sticky)
                    {
                        p.Attach(this);
                    }
                    else
                    {
                        float shuntAmount = MathHelper.Lerp(-3 * MathHelper.PiOver4, -MathHelper.PiOver4, (0.5f + ((b.Body.UserData as Ball).Position.X - p.Body.Position.X) / p.width));
                        Vector2 shuntVector = new Vector2((float)Math.Cos(shuntAmount), (float)Math.Sin(shuntAmount));

                        float shuntStrength = 0.002f;
                        body.ApplyLinearImpulse(shuntVector * shuntStrength);
                    }
                }
            }
            return true;
        }

        public void UpdateSpeed(float speed) {
            this.speed = speed;
        }

        public void UpdateRadius(float radius) {
            this.radius = ConvertUnits.ToSimUnits(radius);
            (body.FixtureList[0].Shape as CircleShape).Radius = this.radius;
        }

        public void Launch(Vector2 direction) {
            body.LinearVelocity = Vector2.Normalize(direction) * speed;
        }

        private bool NearlyVertical(Vector2 v) {
            if (1.0f - Math.Abs(Vector2.Dot(Vector2.Normalize(v), new Vector2(0, 1))) < 0.001f)
                return true;
            return false;
        }

        private bool NearlyHorizontal(Vector2 v)
        {
            if (1.0f - Math.Abs(Vector2.Dot(Vector2.Normalize(v), new Vector2(1, 0))) < 0.001f)
                return true;
            return false;
        }


        public override void Update(GameTime gameTime)
        {
            if (body.LinearVelocity.Length() > 0.001f)
            {

                body.LinearVelocity = Vector2.Normalize(body.LinearVelocity) * speed;
                oldLinearVelocity = body.LinearVelocity;

            }
            else if (oldLinearVelocity != Vector2.Zero)
            {
                body.LinearVelocity = Vector2.Normalize(-oldLinearVelocity) * speed;
            }


            ResolveVerticalAndHorizontalVelocities();

            if (body.Position.Y > maxY)
                Destroy();

            

            trail.Update(gameTime);
            trail.Position = ConvertUnits.ToDisplayUnits(body.Position);
            justCollided = false;
        }

        private void ResolveVerticalAndHorizontalVelocities()
        {
            if (justCollided)
            {
                if (NearlyHorizontal(body.LinearVelocity))
                {
                    if (body.LinearVelocity.X < 0)
                    {
                        float launchAngle = -5 * MathHelper.PiOver4 + (float)Game1.rand.NextDouble() * MathHelper.PiOver4;
                        Vector2 launchVector = new Vector2((float)Math.Cos(launchAngle), (float)Math.Sin(launchAngle));

                        body.LinearVelocity = Vector2.Normalize(launchVector) * speed;
                        oldLinearVelocity = body.LinearVelocity;
                    }
                    else {
                        float launchAngle = -MathHelper.PiOver4 + (float)Game1.rand.NextDouble() * MathHelper.PiOver4;
                        Vector2 launchVector = new Vector2((float)Math.Cos(launchAngle), (float)Math.Sin(launchAngle));

                        body.LinearVelocity = Vector2.Normalize(launchVector) * speed;
                        oldLinearVelocity = body.LinearVelocity;
                    }
                }
                else if (NearlyVertical(body.LinearVelocity)) {
                    if (body.LinearVelocity.Y < 0)
                    {
                        float launchAngle = -3 * MathHelper.PiOver4 + (float)Game1.rand.NextDouble() * MathHelper.PiOver4;
                        Vector2 launchVector = new Vector2((float)Math.Cos(launchAngle), (float)Math.Sin(launchAngle));

                        body.LinearVelocity = Vector2.Normalize(launchVector) * speed;
                        oldLinearVelocity = body.LinearVelocity;
                    }
                    else {
                        float launchAngle = MathHelper.PiOver4 + (float)Game1.rand.NextDouble() * MathHelper.PiOver4;
                        Vector2 launchVector = new Vector2((float)Math.Cos(launchAngle), (float)Math.Sin(launchAngle));

                        body.LinearVelocity = Vector2.Normalize(launchVector) * speed;
                        oldLinearVelocity = body.LinearVelocity;
                    }
                    
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            trail.Draw(spriteBatch);
            switch (player.Index)
            {
                case PlayerIndex.One:
                    spriteBatch.Draw(AssetManager.GetTexture("balls"), ConvertUnits.ToDisplayUnits(Position), new Rectangle(0, 0, 40, 40), Color.White, 0, new Vector2(20.0f, 20.0f), ConvertUnits.ToDisplayUnits(radius) / 20.0f, SpriteEffects.None, 0);
                    break;
                case PlayerIndex.Two:
                    spriteBatch.Draw(AssetManager.GetTexture("balls"), ConvertUnits.ToDisplayUnits(Position), new Rectangle(40, 0, 40, 40), Color.White, 0, new Vector2(20.0f, 20.0f), ConvertUnits.ToDisplayUnits(radius) / 20.0f, SpriteEffects.None, 0);
                    break;
                default:
                    break;
            }
            
        }

    }
}
