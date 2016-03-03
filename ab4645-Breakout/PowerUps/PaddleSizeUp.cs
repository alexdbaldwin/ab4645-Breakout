using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;

namespace ab4645_Breakout.PowerUps
{
    class PaddleSizeUp : PowerUp
    {

        public PaddleSizeUp(GameplayManager gm, World world, Vector2 position)
            : base(gm, world, position, PowerUpType.PaddleSizeUp)
        {

        }

        public override void Apply(PlayerIndex player)
        {
            gm.GetPlayer(player).Paddle.SizeUp();
        }



    }
}
