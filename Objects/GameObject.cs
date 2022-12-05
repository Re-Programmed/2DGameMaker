using _2DGameMaker.Utils.Math;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Objects
{
    public abstract class GameObject
    {
        private GameObject parent;

        private Vec2 position;
        private Vec2 scale;
        private float rotation = 0f;

        private readonly bool alwaysLoad;

        private bool isLoaded;

        public ObjectTexture texture { get; private set; }

        public void SetTexture(ObjectTexture texture) { this.texture = texture; }

        public GameObject(Vec2 position, Vec2 scale, float rotation, ObjectTexture texture, bool alwaysLoad = false, GameObject parent = null)
        {
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
            this.texture = texture;
            this.parent = parent;
            this.alwaysLoad = alwaysLoad;
        }

        public GameObject(Vec2 position, float rotation, ObjectTexture texture, bool alwaysLoad = false, GameObject parent = null)
        {
            this.position = position;
            scale = new Vec2(texture.GetTexture().Width, texture.GetTexture().Height);
            this.rotation = rotation;
            this.texture = texture;
            this.parent = parent;
            this.alwaysLoad = alwaysLoad;
        }

        public GameObject(Vec2 position, ObjectTexture texture, bool alwaysLoad = false, GameObject parent = null)
        {
            this.position = position;
            scale = new Vec2(texture.GetTexture().Width, texture.GetTexture().Height);
            this.texture = texture;
            this.parent = parent;
            this.alwaysLoad = alwaysLoad;
        }

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

        /// <summary>
        /// Rotates some degrees.
        /// </summary>
        /// <param name="rotation">How many degrees to rotate.</param>
        public void Rotate(float rotation)
        {
            this.rotation += rotation;
            this.rotation = Math.ClampSet(this.rotation, 0f, 259f);
        }

        public bool GetAlwaysLoad()
        {
            return alwaysLoad;
        }

        public void SetLoaded(bool loaded)
        {
            if(loaded != isLoaded)
            {
                isLoaded = loaded;
                if (loaded) { OnLoad(); } else { OnDisable(); }
            }
        }

        public bool GetLoaded() { return isLoaded; }

        protected abstract void OnLoad();
        protected abstract void OnDisable();

        
    }
}
