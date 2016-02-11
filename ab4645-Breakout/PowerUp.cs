using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ab4645_Breakout
{
    abstract class PowerUp
    {
        protected float time = -1.0f;
        protected bool expired = false;

        public abstract void Apply(Player player);
        public abstract void Unapply(Player player);

        public virtual void Update(GameTime gameTime)
        {

        }

    }
}
