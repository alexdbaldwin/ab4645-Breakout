using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ab4645_Breakout
{
    class Paddle : GameObject
    {
        protected float width;
        protected float height;

        protected float speed = 10.0f;


        public Paddle(Vector2 position, float width, float height, World world) : base(world) {
            this.width = width;
            this.height = height;

            body = BodyFactory.CreateRectangle(world, width, height, 1, position);
            body.BodyType = BodyType.Kinematic;
            body.CollidesWith = Category.All;
            body.CollisionCategories = Category.All;
            body.Restitution = 1.0f;
            body.Friction = 0.0f;
        }

        public override void Update(GameTime gameTime)
        {
            if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Left))
                body.Position += new Vector2(-1.0f, 0.0f) * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Right))
                body.Position += new Vector2(1.0f, 0.0f) * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetManager.GetTexture("pixel"), ConvertUnits.ToDisplayUnits(Position), null, Color.White, 0, new Vector2(0.5f, 0.5f), ConvertUnits.ToDisplayUnits(new Vector2(width, height)), SpriteEffects.None, 0);
        }
    }
}
