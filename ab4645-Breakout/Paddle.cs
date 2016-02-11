using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
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
        protected PrismaticJoint joint;
        protected PlayerIndex playerIndex;

        protected float movementImpulse = 2.0f;


        protected List<WeldJoint> ballAttachments = new List<WeldJoint>();


        public Paddle(PlayerIndex playerIndex, Vector2 position, float width, float height, float maxTranslation, World world) : base(world) {
            this.playerIndex = playerIndex;
            this.width = width;
            this.height = height;

            body = BodyFactory.CreateRectangle(world, width, height, 1, position);
            body.BodyType = BodyType.Dynamic;
            body.CollidesWith = Category.Cat1;
          
           
            body.CollisionCategories = Category.All;
            body.Restitution = 1.0f;
            body.Friction = 0.0f;
            body.UserData = this;

            Body ground = BodyFactory.CreateRectangle(world, 1, 1, 1);


            joint = JointFactory.CreatePrismaticJoint(world, ground, body, position, new Vector2(1, 0),true);
            joint.LowerLimit = -maxTranslation;
            joint.UpperLimit = maxTranslation;
            joint.LimitEnabled = true;
            body.LinearDamping = 5.0f;
        }

        public void Attach(Ball b) {
            b.Body.Position = body.Position - new Vector2(0, height / 2.0f + b.Radius + 0.5f);
            //b.Body.CollidesWith = Category.None;
            //b.Body.CollisionCategories = Category.Cat2;
            b.Body.Mass = 0.0001f;
            
            ballAttachments.Add(JointFactory.CreateWeldJoint(world, body, b.Body, Vector2.Zero, new Vector2(0, height / 2.0f + b.Radius)));
            float diameterSum = 0;
            foreach (WeldJoint wj in ballAttachments) {
                diameterSum += (wj.BodyB.UserData as Ball).Radius * 2;
            }
            float distanceSoFar = 0;
            for (int i = 0; i < ballAttachments.Count; i++) {
                ballAttachments[i].LocalAnchorB = new Vector2(distanceSoFar - diameterSum/2.0f, height / 2.0f + b.Radius);
                ballAttachments[i].BodyB.Position = body.Position - new Vector2(distanceSoFar - diameterSum / 2.0f, height / 2.0f + b.Radius);
                distanceSoFar += (ballAttachments[i].BodyB.UserData as Ball).Radius * 2;
            }
            //world.RemoveJoint

        }

        private void LaunchBalls() {
            while (ballAttachments.Count > 0) {
                (ballAttachments[0].BodyB.UserData as Ball).Launch(new Vector2(0, -1));
                world.RemoveJoint(ballAttachments[0]);
                ballAttachments.RemoveAt(0);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Left))
                body.ApplyLinearImpulse(new Vector2(-movementImpulse, 0));
            else if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Right))
                body.ApplyLinearImpulse(new Vector2(movementImpulse, 0));

            if (InputHandler.IsButtonDown(playerIndex, PlayerInput.A))
                LaunchBalls();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (playerIndex)
            {
                case PlayerIndex.One:
                    spriteBatch.Draw(AssetManager.GetTexture("paddles"), ConvertUnits.ToDisplayUnits(Position - new Vector2(width, height)/2), new Rectangle(0,0,20,21), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                    spriteBatch.Draw(AssetManager.GetTexture("paddles"), ConvertUnits.ToDisplayUnits(Position - new Vector2(width, height) / 2) + new Vector2(20,0), new Rectangle(20, 0, 3, 21), Color.White, 0, Vector2.Zero, new Vector2((ConvertUnits.ToDisplayUnits(width) - 40)/3.0f,1), SpriteEffects.None, 0);
                    spriteBatch.Draw(AssetManager.GetTexture("paddles"), ConvertUnits.ToDisplayUnits(Position - new Vector2(width, height) / 2) + new Vector2(ConvertUnits.ToDisplayUnits(width) - 20, 0), new Rectangle(23, 0, 20, 21), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                    break;
                case PlayerIndex.Two:
                    spriteBatch.Draw(AssetManager.GetTexture("paddles"), ConvertUnits.ToDisplayUnits(Position), null, Color.White, 0, new Vector2(0.5f, 0.5f), ConvertUnits.ToDisplayUnits(new Vector2(width, height)), SpriteEffects.None, 0);
                    break;
                default:
                    break;
            }
            //spriteBatch.Draw(AssetManager.GetTexture("pixel"), ConvertUnits.ToDisplayUnits(Position), null, Color.White, 0, new Vector2(0.5f, 0.5f), ConvertUnits.ToDisplayUnits(new Vector2(width, height)), SpriteEffects.None, 0);
        }
    }
}
