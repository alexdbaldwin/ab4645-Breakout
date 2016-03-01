using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ab4645_Breakout.PowerUps
{
    class BallSizeUp : PowerUp
    {
        public BallSizeUp(GameplayManager gm, World world, Vector2 position)
            : base(gm, world, position, PowerUpType.BallSizeUp)
        {
        }

        public override void Apply(PlayerIndex player)
        {
            gm.EnlargeBalls(gm.GetPlayer(player));
        }
    }
}
