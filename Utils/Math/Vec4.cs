using System;

namespace _2DGameMaker.Utils.Math
{
    public class Vec4 : Vec3
    {
        public static new readonly Vec4 Zero = new Vec4(0, 0, 0, 0);

        public float W { get; protected set; }

        public Vec4(float X, float Y, float Z, float W)
            :base(X, Y, Z)
        {
            this.W = W;
        }
    }
} 
