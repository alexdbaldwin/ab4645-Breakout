using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ab4645_Breakout.PowerUps
{
    class SplitBalls : PowerUp
    {

        public SplitBalls(GameplayManager gm, World world, Vector2 position)
            : base(gm, world, position, PowerUpType.SplitBalls)
        {
        }

        public override void Apply(PlayerIndex player)
        {
            gm.SplitBalls(gm.GetPlayer(player));
        }
    }
}
