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
    }
}
