using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Collisions;
using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Utils.PhysX.Components
{
    public class DynamicCollisionObject : CollisionObject
    {
        public DynamicCollisionObject(GameObject gameObject, string arg0)
           : base(gameObject)
        {

        }

        protected override void destroy()
        {

        }

        protected override void init()
        {

        }

        protected override void update()
        {
            
        }

        

        internal void CheckCollision(CollisionObject co)
        {
            pushShortestDistance(co);
        }

        private bool axisOverlapCheck(float x1, float x2, float s1, float s2)
        {
            return x1 - s1 / 2 < x2 + s2 / 2 && x1 + s1 / 2 > x2 - s2 / 2;
        }

        private void pushShortestDistance(CollisionObject co)
        {
            Vec2 pos = CollisionCheck.GetCenter(gameObject);
            Vec2 pos2 = co.GetPosition() + co.GetScale() / 2;

            if (!axisOverlapCheck(pos.X, pos2.X, GetScale().X, co.GetScale().X)) { return; }
            if (!axisOverlapCheck(-pos.Y, -pos2.Y, GetScale().Y, co.GetScale().Y)) { return; }

            if (MathF.Abs(pos.X - pos2.X) < MathF.Abs(pos.Y - pos2.Y))
            {
                if (pos.Y < pos2.Y)
                {
                    float q1 = pos.Y + GetScale().Y / 2;
                    float q2 = pos2.Y - co.GetScale().Y / 2;

                    pos.AddY(-performEdgeCheck(q1, q2));
                }
                else
                {
                    float q1 = pos.Y - GetScale().Y / 2;
                    float q2 = pos2.Y + co.GetScale().Y / 2;

                    pos.AddY(performEdgeCheck(q1, q2));
                }
            }
            else
            {
                if(pos.X < pos2.X)
                {
                    float q1 = pos.X + GetScale().X / 2;
                    float q2 = pos2.X - co.GetScale().X / 2;

                    pos.AddX(-performEdgeCheck(q1, q2));
                }
                else
                {
                    float q1 = pos.X - GetScale().X / 2;
                    float q2 = pos2.X + co.GetScale().X / 2;

                    pos.AddX(performEdgeCheck(q1, q2));
                }
            }

            gameObject.SetPosition(pos - GetScale()/2);
        }

        private float performEdgeCheck(float e1, float e2)
        {
            return MathF.Abs(e2 - e1);
        }
    }
}
