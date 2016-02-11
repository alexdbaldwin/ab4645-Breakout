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

            trail = ParticleEmitter.SpawnGenericProjectileEmitter(ConvertUnits.ToDisplayUnits(position));

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
            spriteBatch.Draw(AssetManager.GetTexture("ball"), ConvertUnits.ToDisplayUnits(Position), null, player.BallColor, 0, new Vector2(10.0f, 10.0f), 1, SpriteEffects.None, 0);
        }

    }
}
