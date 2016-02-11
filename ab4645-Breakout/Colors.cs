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
                default:
                    return Color.Black;
            }
        }
    }
}
