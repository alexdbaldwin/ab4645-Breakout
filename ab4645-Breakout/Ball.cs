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


        public Ball(Vector2 position, float radius, World world) : base(world) {
            this.radius = radius;

            body = BodyFactory.CreateCircle(world, radius, 1, position);
            body.BodyType = BodyType.Dynamic;
            body.CollidesWith = Category.All;
            body.CollisionCategories = Category.All;
            body.Restitution = 1.0f;
            body.Friction = 0.0f;

            body.ApplyLinearImpulse(new Vector2(-0.2f, -1f)*0.8f);
            //body.LinearVelocity = new Vector2(-1f, -0.2f)*5f;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetManager.GetTexture("ball"), ConvertUnits.ToDisplayUnits(Position), null, Color.White, 0, new Vector2(10.0f, 10.0f), 1, SpriteEffects.None, 0);
        }

    }
}
