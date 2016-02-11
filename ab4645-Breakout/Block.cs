using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Factories;
using FarseerPhysics;
using FarseerPhysics.Dynamics.Contacts;

namespace ab4645_Breakout
{
    class Block : GameObject
    {
        public enum BlockType
        {
            OneHit,
            TwoHits,
            ThreeHits,
            FourHits,
            FiveHits
        }

        protected float width;
        protected float height;
        protected Color color;
        protected int HP = 1;
        protected BlockType type;


        public Block(Vector2 position, float width, float height, World world, Color color, int HP, BlockType type) : base(world) {
            this.height = height;
            this.width = width;
            this.color = color;
            this.HP = HP;
            this.type = type;

            body = BodyFactory.CreateRectangle(world, width, height, 1, position);
            body.BodyType = BodyType.Static;
            body.CollidesWith = Category.All;
            body.CollisionCategories = Category.All;
            body.Restitution = 1.0f;
            body.Friction = 0.0f;
            body.OnCollision += OnCollision;
            body.UserData = this;


        }

        public void Damage(int amount = 1) {
            HP -= amount;
            if (HP <= 0)
                Destroy();
        }

        public bool OnCollision(Fixture a, Fixture b, Contact c) {
            if (a.Body.UserData is Ball && b.Body.UserData is Block) {
                (b.Body.UserData as Block).Damage();
            }
            else if (a.Body.UserData is Block && b.Body.UserData is Ball)
            {
                (a.Body.UserData as Block).Damage();
            }
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle drawRect = new Rectangle((int)(ConvertUnits.ToDisplayUnits(Position.X - width / 2.0f)), (int)(ConvertUnits.ToDisplayUnits(Position.Y - height / 2.0f)), (int)ConvertUnits.ToDisplayUnits(width), (int)ConvertUnits.ToDisplayUnits(height));
            spriteBatch.Draw(AssetManager.GetTexture("block"), drawRect, new Rectangle(0,0,80,40), Colors.BlockMain(type), 0, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0);
            spriteBatch.Draw(AssetManager.GetTexture("block"), drawRect, new Rectangle(0, 40, 80, 40), Colors.BlockHighlightOne(type), 0, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0);
            spriteBatch.Draw(AssetManager.GetTexture("block"), drawRect, new Rectangle(0, 80, 80, 40), Colors.BlockHighlightTwo(type), 0, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0);
            spriteBatch.Draw(AssetManager.GetTexture("block"), drawRect, new Rectangle(0, 120, 80, 40), Colors.BlockShadowOne(type), 0, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0);
            spriteBatch.Draw(AssetManager.GetTexture("block"), drawRect, new Rectangle(0, 160, 80, 40), Colors.BlockShadowTwo(type), 0, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0);
        }

    }
}
