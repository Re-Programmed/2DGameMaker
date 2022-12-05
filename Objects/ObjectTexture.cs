using _2DGameMaker.Rendering.Sprites;
using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace _2DGameMaker.Objects
{
    public class ObjectTexture
    {
        private bool flipped = false;

        private float textureRotation = 0f;

        private readonly Texture2D texture;

        private Vec4 color;

        public ObjectTexture(Texture2D texture, bool flipped = false, Vec4 color = null, float textureRotation = 0f)
        {
            this.texture = texture;
            this.flipped = flipped;
            this.textureRotation = textureRotation;
            if (color != null) { this.color = color; }
            else { this.color = Vec4.One; }
        }

        public Texture2D GetTexture()
        {
            return texture;
        }

        public void SetFlipped(bool flipped) { this.flipped = flipped; }
        public bool GetFlipped() { return flipped; }

        public void SetRotation(float rotation) { textureRotation = rotation; }
        public float GetRotation() { return textureRotation; }

        public Vec4 GetColor()
        {
            return color;
        }

        public void SetColor(Vec4 color)
        {
            this.color = color;
        }
    }
}
