using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ab4645_Breakout
{
    class Player
    {
        protected int lives = 3;
        protected int balls = 0;
        protected PlayerIndex playerIndex;
        protected GameplayManager gm;
        protected Paddle paddle;
        protected float ballSpeed = 12.5f;
        protected float ballRadius = 10.0f;

        public float BallSpeed { get { return ballSpeed; } set { ballSpeed = value; } }
        public float BallRadius { get { return ballRadius; } set { ballRadius = value; } }

        public Color BallColor {
            get
            {
                switch (playerIndex)
                {
                    case PlayerIndex.One:
                        return Color.Red;
                    case PlayerIndex.Two:
                        return Color.Blue;
                    default:
                        return Color.Black;
                }
            }
        }

        public bool Alive {
            get {
                return lives > 0;
            }
        }

        public int Balls {
            get { return balls; }
        }

        public Paddle Paddle {
            get {
                return paddle;
            }
        }

        public PlayerIndex Index {
            get { return playerIndex; }
        }


        public Player(GameplayManager gm, Paddle paddle, PlayerIndex playerIndex) {
            this.gm = gm;
            this.paddle = paddle;
            this.playerIndex = playerIndex;
        }

        public void RemoveBall() {
            balls--;
            if (balls <= 0)
            {
                lives--;
                if (Alive)
                    gm.SpawnBall(this);
            }  
        }

        public void AddBall() {
            balls++;
        }

        public void AddLife() {
            lives++;
        }

    }
}
