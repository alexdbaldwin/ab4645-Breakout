using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;

namespace ab4645_Breakout
{
    abstract class PowerUp : GameObject
    {
        public enum PowerUpType
        {
            PaddleSizeUp = 0,
            PaddleSizeDown = 1,
            BallSizeUp = 2,
            BallSizeDown = 3,
            SplitBalls = 4,
            BallSpeedUp = 5,
            BallSpeedDown = 6,
            PaddleSpeedUp = 7,
            PaddleSpeedDown = 8,
            //StickyPaddle = 9,
            ExtraBall = 9,
            ExtraLife = 10
        }

        public static readonly int NumberOfPowerUps = 11;

        protected PowerUpType type;
        protected GameplayManager gm;
        bool apply = false;
        PlayerIndex applyToPlayer;

        public PowerUp(GameplayManager gm, World world, Vector2 position, PowerUpType type) : base(world)
        {
            this.gm = gm;
            this.type = type;

            body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(40), ConvertUnits.ToSimUnits(40), 1, position);
            body.BodyType = BodyType.Dynamic;
            body.CollidesWith = Category.Cat2;
            body.CollisionCategories = Category.Cat3;
            body.OnCollision += OnCollision;
            body.UserData = this;

            body.ApplyLinearImpulse(new Vector2(0, 1.0f));
        }

        protected bool OnCollision(Fixture a, Fixture b, Contact c) {
            
            if(a.Body.UserData is Paddle)
            {
                apply = true;
                applyToPlayer = (a.Body.UserData as Paddle).PlayerIndex;
            }
            else if (b.Body.UserData is Paddle)
            {
                apply = true;
                applyToPlayer = (b.Body.UserData as Paddle).PlayerIndex;
            }
            Destroy();
            return true;
        }
        public abstract void Apply(PlayerIndex player);

        public override void Update(GameTime gameTime)
        {
            if (apply)
            {
                AudioManager.PlayPowerUp();
                Apply(applyToPlayer);
                Destroy();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetManager.GetTexture("powerup"), ConvertUnits.ToDisplayUnits(Position), new Rectangle(0, 0, 40, 40), Colors.PowerUpOne(type), 0, new Vector2(20, 20), 1, SpriteEffects.None, 0);
            spriteBatch.Draw(AssetManager.GetTexture("powerup"), ConvertUnits.ToDisplayUnits(Position), new Rectangle(0, 40, 40, 40), Colors.PowerUpTwo(type), 0, new Vector2(20, 20), 1, SpriteEffects.None, 0);
            spriteBatch.Draw(AssetManager.GetTexture("powerup"), ConvertUnits.ToDisplayUnits(Position), new Rectangle(0, 80, 40, 40), Colors.PowerUpThree(type), 0, new Vector2(20, 20), 1, SpriteEffects.None, 0);
        }

    }
}
