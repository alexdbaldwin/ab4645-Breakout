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
        static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();

        public static void AddTexture(string name, Texture2D texture) {
            textures.Add(name, texture);
        }
        public static Texture2D GetTexture(string name) {
            return textures[name];
        }

        public static void AddFont(string name, SpriteFont font)
        {
            fonts.Add(name, font);
        }
        public static SpriteFont GetFont(string name)
        {
            return fonts[name];
        }

    }
}
