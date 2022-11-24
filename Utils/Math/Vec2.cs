using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Utils.Math
{
    public class Vec2
    {
        public static readonly Vec2 Zero = new Vec2(0, 0);

        public float X { get; protected set; }
        public float Y { get; protected set; }

        public Vec2(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }
}
