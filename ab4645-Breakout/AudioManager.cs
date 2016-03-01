using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ab4645_Breakout
{
    static class AudioManager
    {

        public static void PlayBounce() {
            AssetManager.GetSound("bounce").Play();
        }

        public static void PlayDeath() {
            AssetManager.GetSound("death").Play();
        }

        public static void PlayPowerUp() {
            AssetManager.GetSound("powerup").Play();
        }

        public static void PlayMusic() {
            MediaPlayer.Play(AssetManager.GetSong("track1"));
            MediaPlayer.IsRepeating = true;
        }


    }
}
