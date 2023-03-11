using _2DGameMaker.Utils.PhysX.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Utils.PhysX
{
    public static class PhysicsManager
    {
        private static List<CollisionObject> colliders = new List<CollisionObject>();
        private static List<DynamicCollisionObject> dynamic_colliders = new List<DynamicCollisionObject>();

        public static void RegisterCollider(CollisionObject cObject)
        {
            colliders.Add(cObject);
            if(cObject is DynamicCollisionObject)
            {
                dynamic_colliders.Add((DynamicCollisionObject)cObject);
            }
        }

        public static void UnloadCollider(CollisionObject cObject)
        {
            if(colliders.Contains(cObject))
            {
                colliders.Remove(cObject);
            }

            if (cObject is DynamicCollisionObject)
            {
                DynamicCollisionObject dco = (DynamicCollisionObject)cObject;
                if (dynamic_colliders.Contains(dco))
                {
                    dynamic_colliders.Remove(dco);
                }
            }
        }

        //CHUNKS SYSTEM?
        public static void PhysicsTick()
        {
            foreach (DynamicCollisionObject dco in dynamic_colliders)
            {
                foreach(CollisionObject co in colliders)
                {
                    if (dco != co)
                    {
                        dco.CheckCollision(co);
                    }
                }
            }
        }
    }
}
