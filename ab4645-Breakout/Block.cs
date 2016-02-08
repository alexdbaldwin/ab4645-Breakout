using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Factories;
using FarseerPhysics;

namespace ab4645_Breakout
{
    class Block : GameObject
    {
        protected float width;
        protected float height;
        protected Color color;


        public Block(Vector2 position, float width, float height, World world, Color color) : base(world) {
            this.height = height;
            this.width = width;
            this.color = color;

            body = BodyFactory.CreateRectangle(world, width, height, 1, position);
            body.BodyType = BodyType.Static;
            body.CollidesWith = Category.All;
            body.CollisionCategories = Category.All;
            body.Restitution = 1.0f;
            body.Friction = 0.0f;
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetManager.GetTexture("pixel"), ConvertUnits.ToDisplayUnits(Position), null, color, 0, new Vector2(0.5f, 0.5f), ConvertUnits.ToDisplayUnits(new Vector2(width, height)), SpriteEffects.None, 0);
        }

    }
}
