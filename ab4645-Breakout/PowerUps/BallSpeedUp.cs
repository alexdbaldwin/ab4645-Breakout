﻿using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ab4645_Breakout.PowerUps
{
    class BallSpeedUp : PowerUp
    {

        public BallSpeedUp(GameplayManager gm, World world, Vector2 position)
            : base(gm, world, position, PowerUpType.BallSpeedUp)
        {
        }

        public override void Apply(PlayerIndex player)
        {
            gm.SpeedUpBalls(gm.GetPlayer(player));
        }
    }
}
