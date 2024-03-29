﻿using _2DGameMaker.Objects.Scripting;
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

        readonly bool isUI = false;
        public bool IsUI() { return isUI ? true : (parent == null) ? false : parent.IsUI(); }

        public List<ObjectAppendedScript> ObjectAppenedScripts { get; protected set; } = new List<ObjectAppendedScript>();

        public ObjectTexture Texture { get; private set; }

        public void SetTexture(ObjectTexture texture) { this.Texture = texture; }

        public GameObject(Vec2 position, Vec2 scale, float rotation, ObjectTexture texture, bool alwaysLoad = false, GameObject parent = null, bool isUI = false)
        {
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
            this.Texture = texture;
            this.parent = parent;
            this.alwaysLoad = alwaysLoad;
            this.isUI = isUI;
        }

        public GameObject(Vec2 position, float rotation, ObjectTexture texture, bool alwaysLoad = false, GameObject parent = null, bool isUI = false)
        {
            this.position = position;
            scale = new Vec2(texture.GetTexture().Width, texture.GetTexture().Height);
            this.rotation = rotation;
            this.Texture = texture;
            this.parent = parent;
            this.alwaysLoad = alwaysLoad;
            this.isUI = isUI;
        }

        public GameObject(Vec2 position, ObjectTexture texture, bool alwaysLoad = false, GameObject parent = null, bool isUI = false)
        {
            this.position = position;
            scale = new Vec2(texture.GetTexture().Width, texture.GetTexture().Height);
            this.Texture = texture;
            this.parent = parent;
            this.alwaysLoad = alwaysLoad;
            this.isUI = isUI;
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

        public void SetRotation(float rotation)
        {
            this.rotation = rotation;
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

        public void Translate(Vec2 vector)
        {
            this.position += vector;
        }

        public void SetPosition(Vec2 position)
        {
            this.position = position;
        }

        public void SetScale(Vec2 scale)
        {
            this.scale = scale;
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

        internal void AppendScript(ObjectAppendedScript oas)
        {
            ObjectAppenedScripts.Add(oas);
        }

        internal void RemoveScript(ObjectAppendedScript oas)
        {
            ObjectAppenedScripts.Remove(oas);
        }

        public bool GetLoaded() { return isLoaded; }

        protected virtual void OnLoad()
        {
            foreach (ObjectAppendedScript oas in ObjectAppenedScripts)
            {
                oas.OnLoad();
            }
        }

        protected virtual void OnDisable()
        {
            foreach (ObjectAppendedScript oas in ObjectAppenedScripts)
            {
                oas.OnDisable();
            }
        }

        
    }
}
