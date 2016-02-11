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
        protected bool dead = false;

        public Body Body { get { return body; } }

        public bool Dead {
            get { return dead; }
        }

        public Vector2 Position { get { return body.Position; } }

        public GameObject(World world) {
            this.world = world;
        }

        public void Destroy() {
            dead = true;
        }

        public virtual void CleanUp() {
            world.RemoveBody(body);
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);

    }
}
