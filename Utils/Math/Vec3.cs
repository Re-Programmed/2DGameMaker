﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace _2DGameMaker.Utils.Math
{
    public class Vec3 : Vec2
    {
        public static new readonly Vec3 Zero = new Vec3(0, 0, 0);

        public float Z { get; protected set; }

        public Vector3 GetVector()
        {
            return new Vector3(X, Y, Z); 
        }

        public Vec3(float X, float Y, float Z)
            :base(X, Y)
        {
            this.Z = Z;
        }
    }
}
