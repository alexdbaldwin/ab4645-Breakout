using FarseerPhysics;
using FarseerPhysics.Dynamics;
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

        public Player Owner {
            get { return player; }
        }

        public float Radius { get { return radius; } }


        public Ball(Player player, Vector2 position, float radius, float maxY, World world) : base(world) {
            this.player = player;
            this.radius = radius;
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
            body.CollidesWith = Category.All;
            body.CollisionCategories = Category.Cat1;
            body.Restitution = 1.0f;
            body.Friction = 0.0f;

            //body.ApplyLinearImpulse(new Vector2(-0.2f, -1f)*0.8f);
            body.UserData = this;

            //body.LinearVelocity = Vector2.Normalize(new Vector2(-1f, -1f))*10f;
        }

        public void Launch(Vector2 direction) {
            body.LinearVelocity = Vector2.Normalize(direction) * speed;
        }

        public override void Update(GameTime gameTime)
        {
            if (body.LinearVelocity.Length() > 0.001f)
            {
                body.LinearVelocity = Vector2.Normalize(body.LinearVelocity) * speed;
                oldLinearVelocity = body.LinearVelocity;
            }  
            else if(oldLinearVelocity != Vector2.Zero){
                body.LinearVelocity = Vector2.Normalize(-oldLinearVelocity) * speed;
            }
            if (body.Position.Y > maxY)
                Destroy();

            trail.Update(gameTime);
            trail.Position = ConvertUnits.ToDisplayUnits(body.Position);
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
