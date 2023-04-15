using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Stationaries;
using _2DGameMaker.Utils.AssetManagment;
using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.GAME_NAME.GUI
{
    public static class NumberRenderer
    {
        private const string NUMBER_TEXTURE_FILE = "ui_defaults";
        private const string NUMBER_TEXTURE_PREFIX = "number_";

        /// <summary>
        /// Returns the object texture for a given one digit integer.
        /// </summary>
        /// <param name="d">One digit integer.</param>
        /// <returns>An ObjectTexture representing the byte d.</returns>
        private static ObjectTexture getTextureFromSingleDigit(string d)
        {
            return new ObjectTexture(AssetManager.GetTexture(NUMBER_TEXTURE_PREFIX + d.ToString(), NUMBER_TEXTURE_FILE));
        }

        /// <summary>
        /// Returns the game object of a number based on the given one digit integer.
        /// </summary>
        /// <param name="number">A one digit integer.</param>
        public static UINumberObject GetSingleNumber(char number, Vec2 position, float scale /* Preferably a multiple of the scale of the number textures. */, GameObject parent = null)
        {
            return new UINumberObject(position, Vec2.One * scale, getTextureFromSingleDigit(number.ToString()), parent);
        }

        /// <summary>
        /// Returns the game object of a number based on the given digit.
        /// </summary>
        /// <param name="number">The number to set as the game object's texture.</param>
        /// <param name="position">The upper left corner of the number.</param>
        /// <param name="scale">The size of the number.</param>
        public static UINumberObject GetNumber(int number, Vec2 position, float scale, out UINumberObject[] objects, int layer = 3, int padWithZeros = 0, float padding = -12f)
        {
            UINumberObject parentObject = new UINumberObject(position, Vec2.One, null);

            Vec2 currentPosition = Vec2.Zero;
            List<char> draws = new List<char>();

            string str = number.ToString();

            padWithZeros -= str.Length;
            if(padWithZeros > 0)
            {
                for (int i = 0; i < padWithZeros; i++) { draws.Add('0'); }

                draws.AddRange(str.ToCharArray());
            }

            objects = new UINumberObject[draws.Count]; 

            for(int i = 0; i < draws.Count; ++i)
            {
                objects[i] = GetSingleNumber(draws[i], currentPosition.Clone(), scale, parentObject);
                Game.Game.INSTANCE.Instantiate(objects[i], layer);
                currentPosition.AddX(scale + padding);
            }

            return parentObject;
        }
    }
}
