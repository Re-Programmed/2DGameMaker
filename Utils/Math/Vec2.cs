using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Numerics;
using System.Text;

namespace _2DGameMaker.Utils.Math
{
    public class Vec2
    {
        public static Vec2 Zero
        { get => new Vec2(0, 0); }

        public static Vec2 One
        { get => new Vec2(1, 1); }

        public static  Vec2 OneX
        { get => new Vec2(1, 0); }

        public static Vec2 OneY
        { get => new Vec2(0, 1); }

        public float X { get; protected set; }
        public float Y { get; protected set; }

        public Vec2(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public Vec2(Vector2 vec)
        {
            this.X = vec.X;
            this.Y = vec.Y;
        }

        public static Vec2 operator +(Vec2 a, Vec2 b) => new Vec2(a.X + b.X, a.Y + b.Y);
        public static Vec2 operator +(Vec2 a, float b) => new Vec2(a.X + b, a.Y + b);
        public static Vec2 operator *(Vec2 a, Vec2 b) => new Vec2(a.X * b.X, a.Y * b.Y);
        public static Vec2 operator *(Vec2 a, float b) => new Vec2(a.X * b, a.Y * b);
        public static Vec2 operator /(Vec2 a, float b) => new Vec2(a.X / b, a.Y / b);
        public static Vec2 operator /(Vec2 a, Vec2 b) => new Vec2(a.X / b.X, a.Y / b.Y);
        public static Vec2 operator -(Vec2 a, Vec2 b) => new Vec2(a.X - b.X, a.Y - b.Y);
        public static Vec2 operator -(Vec2 a) => new Vec2(-a.X, -a.Y);
        public static Vec2 operator -(Vec2 a, float b) => new Vec2(a.X - b, a.Y - b);


        public Vec2 Lerp(Vec2 target, float speed)
        {
            X = Math.Lerp(X, target.X, speed);
            Y = Math.Lerp(Y, target.Y, speed);

            return this;
        }

        public static Vec2 InverseY(Vec2 vec2)
        {
            return new Vec2(vec2.X, -vec2.Y);
        }

        public void AddY(float value)
        {
            Y += value;
        }

        public void AddX(float value)
        {
            X += value;
        }

        public void SetX(float value)
        {
            X = value;
        }

        public void SetY(float value)
        {
            Y = value;
        }

        public float[] GetArray()
        {
            return new float[2] { X, Y };
        }

        public override string ToString()
        {
            return "Vec2 [" + X + ", " + Y + "]";
        }

        public Vec2 Clone()
        {
            return new Vec2(X, Y);
        }
    }
}
