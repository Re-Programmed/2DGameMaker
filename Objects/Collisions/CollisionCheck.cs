using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Objects.Collisions
{
    class CollisionCheck
    {
        public static bool BoxCheck(Vec2 collider, Vec2 box_center, Vec2 box_scale)
        {
            return CheckValueBetweenScales(collider.X, box_center.X, box_scale.X) && CheckValueBetweenScales(collider.Y, box_center.Y, box_scale.Y);
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
    }
}
