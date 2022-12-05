using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace _2DGameMaker.Utils.Math
{
    static class Math
    {
        public static readonly Vector2 RightVector = new Vector2(1, 0);
        public static readonly Vector2 LeftVector = new Vector2(-1, 0);
        public static readonly Vector2 UpVector = new Vector2(0, -1);
        public static readonly Vector2 DownVector = new Vector2(0, 1);

        public static readonly float PI = MathF.PI;

        public static float DegToRad(float degrees)
        {
            return degrees * PI / 180;
        }

        public static byte LimitIntToByte(int input)
        {
            input = System.Math.Clamp(input, 0, 255);

            return (byte)input;
        }

        public static float Lerp(float value, float secondValue, float by)
        {
            return value * (1 - by) + secondValue * by;
        }

        /// <summary>
        /// Wraps value to lower limit if it is greater than the upper limit and vice versa.
        /// </summary>
        /// <param name="value">Value to wrap.</param>
        /// <param name="LowerLimit">Lower limit.</param>
        /// <param name="UpperLimit">Upper limit.</param>
        /// <returns>Wrapped value.</returns>
        public static float ClampSet(float value, float lowerLimit, float upperLimit)
        {
            if(value > upperLimit)
            {
                 return lowerLimit;
            }
            
            if(value < lowerLimit)
            {
                return upperLimit;
            }

            return value;
        }
    }
}
