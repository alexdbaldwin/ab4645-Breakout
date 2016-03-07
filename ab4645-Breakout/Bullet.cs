using FarseerPhysics;
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
    class Bullet : GameObject
    {
        protected ParticleEmitter trail;

        public Bullet(Vector2 position, World world) : base(world) {

            trail = ParticleEmitter.SpawnBulletTrail(ConvertUnits.ToDisplayUnits(position));

            body = BodyFactory.CreateCircle(world, ConvertUnits.ToSimUnits(0.5f), 1, position);
            body.BodyType = BodyType.Dynamic;
            body.CollidesWith = Category.Cat4;
            body.CollisionCategories = Category.Cat5;
            body.Restitution = 1.0f;
            body.Friction = 0.0f;
            body.OnCollision += OnCollision;
            body.LinearVelocity = new Vector2(0, -25);

            body.UserData = this;

        }

        public override void Update(GameTime gameTime)
        {
            trail.Update(gameTime);
            trail.Position = ConvertUnits.ToDisplayUnits(Position);
            if (Position.Y < 0)
                Destroy();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            trail.Draw(spriteBatch);
        }

        public bool OnCollision(Fixture a, Fixture b, Contact c)
        {
            if (b.Body.UserData is Block)
            {
                (b.Body.UserData as Block).Damage();
            }
            else if (a.Body.UserData is Block)
            {
                (a.Body.UserData as Block).Damage();
            }
            Destroy();
            return true;
        }
    }
}
