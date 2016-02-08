using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ab4645_Breakout
{
    static class AssetManager
    {

        static Dictionary<string,Texture2D> textures = new Dictionary<string,Texture2D>();

        public static void AddTexture(string name, Texture2D texture) {
            textures.Add(name, texture);
        }
        public static Texture2D GetTexture(string name) {
            return textures[name];
        }

    }
}
