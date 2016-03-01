using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
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
        static Dictionary<string, Song> songs = new Dictionary<string, Song>();
        static Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

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

        public static void AddSong(string name, Song song)
        {
            songs.Add(name, song);
        }
        public static Song GetSong(string name)
        {
            return songs[name];
        }

        public static void AddSound(string name, SoundEffect sound)
        {
            sounds.Add(name, sound);
        }
        public static SoundEffect GetSound(string name)
        {
            return sounds[name];
        }



    }
}
