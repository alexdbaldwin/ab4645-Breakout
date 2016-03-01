using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ab4645_Breakout.PowerUps
{
    class PaddleSpeedDown : PowerUp
    {
        public PaddleSpeedDown(GameplayManager gm, World world, Vector2 position)
            : base(gm, world, position, PowerUpType.PaddleSpeedDown)
        {
        }

        public override void Apply(PlayerIndex player)
        {
            gm.GetPlayer(player).Paddle.SpeedDown();
        }

    }
}
