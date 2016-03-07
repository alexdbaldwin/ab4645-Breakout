using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ab4645_Breakout
{
    static class Colors
    {

        public static Color BlockMain(Block.BlockType type) {
            switch (type)
            {
                case Block.BlockType.OneHit:
                    return new Color(255, 255, 89, 255);
                case Block.BlockType.TwoHits:
                    return new Color(255, 175, 71, 255);
                case Block.BlockType.ThreeHits:
                    return new Color(255, 56, 135, 255);
                case Block.BlockType.FourHits:
                    return new Color(149, 73, 255, 255);
                case Block.BlockType.FiveHits:
                    return new Color(73, 255, 164, 255);
                case Block.BlockType.Indestructible:
                    return new Color(20, 20, 20, 255);
                default:
                    return Color.Black;
            }
        }

        public static Color BlockHighlightOne(Block.BlockType type)
        {
            switch (type)
            {
                case Block.BlockType.OneHit:
                    return new Color(255, 255, 153, 255);
                case Block.BlockType.TwoHits:
                    return new Color(255, 210, 153, 255);
                case Block.BlockType.ThreeHits:
                    return new Color(255, 130, 180, 255);
                case Block.BlockType.FourHits:
                    return new Color(191, 145, 255, 255);
                case Block.BlockType.FiveHits:
                    return new Color(130, 255, 192, 255);
                case Block.BlockType.Indestructible:
                    return new Color(40, 40, 40, 255);
                default:
                    return Color.Black;
            }
        }

        public static Color BlockHighlightTwo(Block.BlockType type)
        {
            switch (type)
            {
                case Block.BlockType.OneHit:
                    return new Color(255, 255, 216, 255);
                case Block.BlockType.TwoHits:
                    return new Color(255, 237, 214, 255);
                case Block.BlockType.ThreeHits:
                    return new Color(255, 204, 224, 255);
                case Block.BlockType.FourHits:
                    return new Color(225, 204, 255, 255);
                case Block.BlockType.FiveHits:
                    return new Color(216, 255, 235, 255);
                case Block.BlockType.Indestructible:
                    return new Color(30, 30, 30, 255);
                default:
                    return Color.Black;
            }
        }

        public static Color BlockShadowOne(Block.BlockType type)
        {
            switch (type)
            {
                case Block.BlockType.OneHit:
                    return new Color(137, 137, 78, 255);
                case Block.BlockType.TwoHits:
                    return new Color(142, 109, 65, 255);
                case Block.BlockType.ThreeHits:
                    return new Color(112, 68, 85, 255);
                case Block.BlockType.FourHits:
                    return new Color(88, 65, 119, 255);
                case Block.BlockType.FiveHits:
                    return new Color(56, 99, 78, 255);
                case Block.BlockType.Indestructible:
                    return new Color(10, 10, 10, 255);
                default:
                    return Color.Black;
            }
        }

        public static Color BlockShadowTwo(Block.BlockType type)
        {
            switch (type)
            {
                case Block.BlockType.OneHit:
                    return new Color(181, 181, 74, 255);
                case Block.BlockType.TwoHits:
                    return new Color(193, 145, 81, 255);
                case Block.BlockType.ThreeHits:
                    return new Color(160, 97, 123, 255);
                case Block.BlockType.FourHits:
                    return new Color(124, 92, 168, 255);
                case Block.BlockType.FiveHits:
                    return new Color(88, 155, 122, 255);
                case Block.BlockType.Indestructible:
                    return new Color(0, 0, 0, 255);
                default:
                    return Color.Black;
            }
        }

        public static Color PowerUpOne(PowerUp.PowerUpType type) {
            switch (type)
            {
                case PowerUp.PowerUpType.PaddleSizeUp:
                    return Color.LightYellow;
                case PowerUp.PowerUpType.PaddleSizeDown:
                    return Color.LightGreen;
                case PowerUp.PowerUpType.BallSizeUp:
                    return Color.LightSalmon;
                //case PowerUp.PowerUpType.BallSizeDown:
                //    return Color.MediumPurple;
                case PowerUp.PowerUpType.SplitBalls:
                    return Color.PaleVioletRed;
                case PowerUp.PowerUpType.BallSpeedUp:
                    return Color.LightBlue;
                //case PowerUp.PowerUpType.BallSpeedDown:
                //    return Color.LightPink;
                case PowerUp.PowerUpType.PaddleSpeedUp:
                    return Color.LightCyan;
                case PowerUp.PowerUpType.PaddleSpeedDown:
                    return Color.FloralWhite;
                //case PowerUp.PowerUpType.StickyPaddle:
                //    return Color.LightGray;
                case PowerUp.PowerUpType.ExtraBall:
                    return Color.White;
                case PowerUp.PowerUpType.ExtraLife:
                    return Color.Beige;
                case PowerUp.PowerUpType.TemporaryGun:
                    return Color.LightGray;
                default:
                    return Color.Brown;
            }

        }

        public static Color PowerUpTwo(PowerUp.PowerUpType type)
        {
            switch (type)
            {
                case PowerUp.PowerUpType.PaddleSizeUp:
                    return Color.Yellow;
                case PowerUp.PowerUpType.PaddleSizeDown:
                    return Color.Green;
                case PowerUp.PowerUpType.BallSizeUp:
                    return Color.Orange;
                //case PowerUp.PowerUpType.BallSizeDown:
                //    return Color.Purple;
                case PowerUp.PowerUpType.SplitBalls:
                    return Color.Red;
                case PowerUp.PowerUpType.BallSpeedUp:
                    return Color.Blue;
                //case PowerUp.PowerUpType.BallSpeedDown:
                //    return Color.HotPink;
                case PowerUp.PowerUpType.PaddleSpeedUp:
                    return Color.Cyan;
                case PowerUp.PowerUpType.PaddleSpeedDown:
                    return Color.Magenta;
                //case PowerUp.PowerUpType.StickyPaddle:
                //    return Color.DarkGray;
                case PowerUp.PowerUpType.ExtraBall:
                    return Color.White;
                case PowerUp.PowerUpType.ExtraLife:
                    return Color.SandyBrown;
                case PowerUp.PowerUpType.TemporaryGun:
                    return Color.DarkGray;
                default:
                    return Color.Brown;
            }
        }

        public static Color PowerUpThree(PowerUp.PowerUpType type)
        {
            switch (type)
            {
                case PowerUp.PowerUpType.PaddleSizeUp:
                    return Color.DarkGoldenrod;
                case PowerUp.PowerUpType.PaddleSizeDown:
                    return Color.DarkGreen;
                case PowerUp.PowerUpType.BallSizeUp:
                    return Color.DarkOrange;
                //case PowerUp.PowerUpType.BallSizeDown:
                //    return Color.DarkViolet;
                case PowerUp.PowerUpType.SplitBalls:
                    return Color.DarkRed;
                case PowerUp.PowerUpType.BallSpeedUp:
                    return Color.DarkBlue;
                //case PowerUp.PowerUpType.BallSpeedDown:
                //    return Color.DeepPink;
                case PowerUp.PowerUpType.PaddleSpeedUp:
                    return Color.DarkCyan;
                case PowerUp.PowerUpType.PaddleSpeedDown:
                    return Color.DarkMagenta;
                //case PowerUp.PowerUpType.StickyPaddle:
                //    return Color.Black;
                case PowerUp.PowerUpType.ExtraBall:
                    return Color.White;
                case PowerUp.PowerUpType.ExtraLife:
                    return Color.Brown;
                case PowerUp.PowerUpType.TemporaryGun:
                    return Color.Black;
                default:
                    return Color.Brown;
            }
        }
    }
}
