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
        public float width;
        protected float height;
        protected GameplayManager gm;

        protected float speed = 10.0f;
        protected PrismaticJoint joint;
        protected PlayerIndex playerIndex;
        Body ground;
        float maxTranslation;
        Vector2 startPos;
        bool sticky = false;
        protected float movementImpulse = 2.0f;
        protected float movementImpulseMin = 0.5f;
        protected float movementImpulseMax = 4.0f;
        bool gun = false;
        float gunTimer = 10.0f;
        float gunShotDelay = 0.5f;
        float gunShotTimer = 0.5f;

        public PlayerIndex PlayerIndex { get { return playerIndex; } }
        public bool Sticky { get { return sticky; } }


        protected List<WeldJoint> ballAttachments = new List<WeldJoint>();


        public Paddle(PlayerIndex playerIndex, GameplayManager gm, Vector2 position, float width, float height, float maxTranslation, World world) : base(world) {
            this.playerIndex = playerIndex;
            this.gm = gm;
            this.width = width;
            this.height = height;
            this.maxTranslation = maxTranslation;
            startPos = new Vector2(position.X,position.Y) ;

            body = BodyFactory.CreateRectangle(world, width, height, 1, position);
            body.BodyType = BodyType.Dynamic;
            body.CollidesWith = Category.Cat1 | Category.Cat3;
            body.CollisionCategories = Category.Cat2;
            body.Restitution = 1.0f;
            body.Friction = 0.0f;
            body.UserData = this;

            ground = BodyFactory.CreateRectangle(world, 1, 1, 1);


            joint = JointFactory.CreatePrismaticJoint(world, ground, body, position, new Vector2(1, 0),true);
            joint.LowerLimit = -maxTranslation + width / 2.0f;
            joint.UpperLimit = maxTranslation - width / 2.0f;
            joint.LimitEnabled = true;
            body.LinearDamping = 7.0f;

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
                ballAttachments[i].LocalAnchorB = new Vector2(distanceSoFar - diameterSum/2.0f + (ballAttachments[i].BodyB.UserData as Ball).Radius, height / 2.0f + (ballAttachments[i].BodyB.UserData as Ball).Radius);
                ballAttachments[i].BodyB.Position = body.Position - new Vector2(distanceSoFar - diameterSum / 2.0f, height / 2.0f + (ballAttachments[i].BodyB.UserData as Ball).Radius);
                distanceSoFar += (ballAttachments[i].BodyB.UserData as Ball).Radius * 2;
            }
            //world.RemoveJoint

        }

        private void LaunchBalls() {
            while (ballAttachments.Count > 0) {
                //Calculate launch angle...
                float launchAngle = MathHelper.Lerp(-3 * MathHelper.PiOver4, -MathHelper.PiOver4, ((ballAttachments[0].BodyB.UserData as Ball).Position.X - (body.Position.X - width / 2.0f)) / width);
                Vector2 launchVector = new Vector2((float)Math.Cos(launchAngle), (float)Math.Sin(launchAngle));

                (ballAttachments[0].BodyB.UserData as Ball).Launch(launchVector);
                world.RemoveJoint(ballAttachments[0]);
                ballAttachments.RemoveAt(0);
            }
        }

        public void MakeSticky() {
            sticky = !sticky;
        }

        public void SpeedUp() {
            movementImpulse = MathHelper.Clamp(movementImpulse * 1.33f, movementImpulseMin, movementImpulseMax);
        }

        public void SpeedDown() {
            movementImpulse = MathHelper.Clamp(movementImpulse * 0.75f, movementImpulseMin, movementImpulseMax);
        }

        public void SizeUp() {
            Resize(1.25f);
        }

        public void SizeDown()
        {
            Resize(0.8f);
        }

        private void Resize(float scaleFactor) {
            width *= scaleFactor;
            List<Ball> tmpBalls = new List<Ball>();
            while (ballAttachments.Count > 0)
            {
                tmpBalls.Add(ballAttachments[0].BodyB.UserData as Ball);
                world.RemoveJoint(ballAttachments[0]);
                ballAttachments.RemoveAt(0);
            }

            world.RemoveJoint(joint);

            
            Vector2 pos = body.Position;
            Vector2 linearVel = body.LinearVelocity;
            float mass = body.Mass;
            world.RemoveBody(body);
            body = BodyFactory.CreateRectangle(world, width, height, 1, pos);
            body.BodyType = BodyType.Dynamic;
            body.CollidesWith = Category.Cat1 | Category.Cat3;
            body.CollisionCategories = Category.Cat2;
            body.Restitution = 1.0f;
            body.Friction = 0.0f;
            body.UserData = this;
            body.Mass = mass;
            body.LinearVelocity = linearVel;



            joint = JointFactory.CreatePrismaticJoint(world, ground, body, startPos, new Vector2(1, 0), true);
            joint.LowerLimit = -maxTranslation + width / 2.0f + (startPos.X - pos.X);
            joint.UpperLimit = maxTranslation - width / 2.0f + (startPos.X - pos.X);
            joint.LimitEnabled = true;
            body.LinearDamping = 5.0f;

            foreach (Ball b in tmpBalls)
                Attach(b);
        }

        

        public override void Update(GameTime gameTime)
        {
            if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Left))
                body.ApplyLinearImpulse(new Vector2(-movementImpulse, 0));
            else if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Right))
                body.ApplyLinearImpulse(new Vector2(movementImpulse, 0));

            


            if (gun)
            {
                gunShotTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                gunTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (gunTimer <= 0)
                {
                    gun = false;
                }
            }

            if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Red)){
                LaunchBalls();
                if(gun){
                    if (gunShotTimer <= 0) {
                        Shoot();
                        gunShotTimer = gunShotDelay;
                    }
                }
            }
        }

        private void Shoot() {
            gm.SpawnBullet(Position - new Vector2(width, height) / 2 + ConvertUnits.ToSimUnits(new Vector2(13.5f, -7)));
            gm.SpawnBullet(Position + new Vector2(width, -height) / 2 + ConvertUnits.ToSimUnits(new Vector2(-13.5f, -7)));
        }

        public void ActivateGun(float time)
        {
            gun = true;
            gunTimer = time;

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
                    spriteBatch.Draw(AssetManager.GetTexture("paddles"), ConvertUnits.ToDisplayUnits(Position - new Vector2(width, height)/2), new Rectangle(43,0,20,21), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                    spriteBatch.Draw(AssetManager.GetTexture("paddles"), ConvertUnits.ToDisplayUnits(Position - new Vector2(width, height) / 2) + new Vector2(20,0), new Rectangle(63, 0, 3, 21), Color.White, 0, Vector2.Zero, new Vector2((ConvertUnits.ToDisplayUnits(width) - 40)/3.0f,1), SpriteEffects.None, 0);
                    spriteBatch.Draw(AssetManager.GetTexture("paddles"), ConvertUnits.ToDisplayUnits(Position - new Vector2(width, height) / 2) + new Vector2(ConvertUnits.ToDisplayUnits(width) - 20, 0), new Rectangle(66, 0, 20, 21), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                    break;
                default:
                    break;
            }
            if (gun) {
                spriteBatch.Draw(AssetManager.GetTexture("gun"), ConvertUnits.ToDisplayUnits(Position - new Vector2(width, height) / 2) + new Vector2(10, -7), Color.White);
                spriteBatch.Draw(AssetManager.GetTexture("gun"), ConvertUnits.ToDisplayUnits(Position + new Vector2(width, -height) / 2) + new Vector2(-17, -7), Color.White);
            }
            //spriteBatch.Draw(AssetManager.GetTexture("pixel"), ConvertUnits.ToDisplayUnits(Position), null, Color.White, 0, new Vector2(0.5f, 0.5f), ConvertUnits.ToDisplayUnits(new Vector2(width, height)), SpriteEffects.None, 0);
        }
    }
}
