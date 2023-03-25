using System;

namespace _2DGameMaker.Utils.Math
{
    public class Vec4 : Vec3
    {
        public static new readonly Vec4 Zero = new Vec4(0, 0, 0, 0);
        public static new readonly Vec4 One = new Vec4(1, 1, 1, 1);
        public static readonly Vec4 Red = new Vec4(1, 0, 0, 1);
        public static readonly Vec4 Green = new Vec4(0, 1, 0, 1);
        public static readonly Vec4 Blue = new Vec4(0, 0, 1, 1);

        public float W { get; protected set; }

        public Vec4(float X, float Y, float Z, float W)
            :base(X, Y, Z)
        {
            this.W = W;
        }

        /// <summary>
        /// Takes in a hexadecimal color code (rgba) and converts it to a Vec4.
        /// </summary>
        /// <param name="c">Hexadecimal color code (rgba)</param>
        /// <returns>A Vec4 color.</returns>
        public static Vec4 StringToColor(string c)
        {
            string[] rgba = new string[4];

            int i = 0;
            foreach(char ch in c)
            {
                rgba[(int)MathF.Floor(i / 2f)] += ch;
                i++;
            }

            return new Vec4(hex2dToInt(rgba[0]), hex2dToInt(rgba[1]), hex2dToInt(rgba[2]), hex2dToInt(rgba[3]));
        }

        /// <summary>
        /// Converts a 2 digit hex number - represented as a string - to decimal.
        /// </summary>
        private static int hex2dToInt(string hex)
        {
            char[] chars = hex.ToCharArray();
            return singleHexDigitToInt(chars[1]) + singleHexDigitToInt(chars[0]) * 16;
        }

        /// <summary>
        /// Converts a char to its hexadecimal value.
        /// </summary>
        private static int singleHexDigitToInt(char c)
        {
            int res;
            if (int.TryParse(c.ToString(), out res))
            {
                return res;
            }

            switch (c)
            {
                case 'A':
                    return 10;
                case 'B':
                    return 11;
                case 'C':
                    return 12;
                case 'D':
                    return 13;
                case 'E':
                    return 14;
                case 'F':
                    return 15;
            }

            return -1;
        }

        public override string ToString()
        {
            return $"Vec4 [{X}, {Y}, {Z}, {W}]";
        }
    }
} 
