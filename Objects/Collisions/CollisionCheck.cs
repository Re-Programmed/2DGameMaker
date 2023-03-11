using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Objects.Collisions
{
    class CollisionCheck
    {
        /// <summary>
        /// Returns the center of a game object.
        /// </summary>
        /// <param name="obj">The object to find the center of.</param>
        /// <returns>The center.</returns>
        public static Vec2 GetCenter(GameObject obj)
        {
            return new Vec2(obj.GetPosition().X + obj.GetScale().X/2, obj.GetPosition().Y + obj.GetScale().Y/2);
        }

        /// <summary>
        /// Check if two boxes overlap.
        /// </summary>
        /// <param name="colliderCenter">The first box position.</param>
        /// <param name="colliderScale">The first box scale.</param>
        /// <param name="collider2Center">The second box position.</param>
        /// <param name="collider2Scale">The second box scale.</param>
        /// <returns>If the boxes overlap.</returns>
        public static bool BoxOverlapCheck(Vec2 colliderCenter, Vec2 colliderScale, Vec2 collider2Center, Vec2 collider2Scale)
        {
            return CheckValueBetween2Scales(colliderCenter.X, colliderScale.X, collider2Center.X, collider2Scale.X) && CheckValueBetween2Scales(colliderCenter.Y, colliderScale.Y, collider2Center.Y, collider2Scale.Y);
        }

        /// <summary>
        /// Checks if a point is within a box.
        /// </summary>
        /// <param name="collider">The point to check.</param>
        /// <param name="boxCenter">The box center.</param>
        /// <param name="boxScale">The box scale.</param>
        /// <returns>If the point is within the box.</returns>
        public static bool BoxCheck(Vec2 collider, Vec2 boxCenter, Vec2 boxScale)
        {
            return CheckValueBetweenScales(collider.X, boxCenter.X, boxScale.X) && CheckValueBetweenScales(collider.Y, boxCenter.Y, boxScale.Y);
        }

        /// <summary>
        /// Returns the distance between two vectors.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static float Distance(Vec2 p1, Vec2 p2)
        {
            return MathF.Sqrt(MathF.Pow(MathF.Abs(p1.X - p2.X) + MathF.Abs(p1.Y - p2.Y), 2));
        }

        /// <summary>
        /// Checks if a point is within an objects dimensions.
        /// </summary>
        /// <param name="collider">The point to check.</param>
        /// <param name="obj">The object.</param>
        /// <param name="p5">Add /2 to Y padding.</param>
        /// <returns>If the point is within the object.</returns>
        public static bool BoxCheck(Vec2 collider, GameObject obj, bool p5 = false)
        {
            Vec2 center = GetCenter(obj);
            return CheckValueBetweenScales(collider.X, center.X, obj.GetScale().X) && CheckValueBetweenScales(collider.Y - (p5 ? obj.GetScale().Y/2 : 0), center.Y, obj.GetScale().Y);
        }

        /// <summary>
        /// Checks if a value is within a certain distance of a different value.
        /// </summary>
        /// <param name="v1">Check value.</param>
        /// <param name="v2">Value to compare against.</param>
        /// <param name="s">Distance from v2.</param>
        /// <returns>If a value is within a certain distance of a different value.</returns>
        public static bool CheckValueBetweenScales(float v1, float v2, float s)
        {
            return v1 < v2 + s/2 && v1 > v2 - s/2;
        }

        public static bool CheckValueBetween2Scales(float v1, float s1, float v2, float s2)
        {
            return v1 - s1 / 2 < v2 + s2 / 2 && v1 + s1 / 2 > v2 - s2 / 2;
        }


    }
}
