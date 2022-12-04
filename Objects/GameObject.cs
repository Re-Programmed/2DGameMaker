using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Objects
{
    abstract class GameObject
    {
        private GameObject parent;

        private Vec2 position;
        private Vec2 scale;
        private float rotation;

        public ObjectTexture texture { get; private set; }

        public Vec2 GetPosition()
        {
            if (parent != null) { return parent.GetPosition() + position; }
            return position;
        }
        public Vec2 GetLocalPosition() { return position; }

        public Vec2 GetScale() 
        {
            if (parent != null) { return parent.GetScale() * scale; }
            return scale;
        }
        public Vec2 GetLocalScale() { return scale; }

        public float GetRotation()
        {
            if (parent != null) { return parent.GetRotation() + rotation; }
            return rotation;
        }
        public float GetLocalRotation() { return rotation; }
    }
}
