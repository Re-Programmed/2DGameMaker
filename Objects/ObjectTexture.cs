using _2DGameMaker.Rendering.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Objects
{
    class ObjectTexture
    {
        bool flipped = false;

        float textureRotation = 0f;

        private readonly Texture2D texture;

        public ObjectTexture(Texture2D texture, bool flipped = false, float textureRotation = 0f)
        {
            this.texture = texture;
            this.flipped = flipped;
            this.textureRotation = textureRotation;
        }

        public Texture2D GetTexture()
        {
            return texture;
        }

        public void SetFlipped(bool flipped) { this.flipped = flipped; }
        public bool GetFlipped() { return flipped; }

        public void SetRotation(float rotation) { textureRotation = rotation; }
        public float GetRotation() { return textureRotation; }
    }
}
