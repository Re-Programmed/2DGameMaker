using _2DGameMaker.Rendering.Sprites;
using _2DGameMaker.Utils.AssetManagment;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Utils.Animatr
{
    public static class AnimationBuilder
    {
        /// <summary>
        /// Generates a list of textures from a 2d string array.
        /// </summary>
        public static Texture2D[] BuildT2D(string[,] frames /*Texture directory, name*/)
        {
            Texture2D[] ret = new Texture2D[frames.Length / 2];

            for (int i = 0; i < frames.Length / 2; i++)
            {
                ret[i] = AssetManager.GetTexture(frames[i, 0], frames[i, 1]);
            }

            return ret;
        }
    }
}
