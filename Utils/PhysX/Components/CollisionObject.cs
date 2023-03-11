using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Scripting;
using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Utils.PhysX.Components
{
    public abstract class CollisionObject : ObjectAppendedScript
    {
        public CollisionObject(GameObject gameObject)
            : base(gameObject)
        { }

        public override void OnLoad()
        {
            PhysicsManager.RegisterCollider(this);
        }

        public override void OnDisable()
        {
            PhysicsManager.UnloadCollider(this);
        }

        public Vec2 GetPosition()
        {
            return gameObject.GetPosition();
        }

        public Vec2 GetScale()
        {
            return gameObject.GetScale();
        }
    }
}
