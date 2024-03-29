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
    }
} 
