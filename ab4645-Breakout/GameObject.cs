using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ab4645_Breakout
{
    abstract class GameObject
    {
        protected Body body;
        protected World world;

        public Vector2 Position { get { return body.Position; } }

        public GameObject(World world) {
            this.world = world;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);

    }
}
