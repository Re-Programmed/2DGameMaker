using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Scripting;
using _2DGameMaker.Rendering.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Utils.Animatr
{
    public abstract class Animation : ObjectAppendedScript
    {
        private readonly Texture2D[] frames;
        private int currentFrame = 0;

        public Animation(GameObject gameObject, Texture2D[] frames)
            : base(gameObject)
        {
            this.frames = frames;
        }

        protected void advanceFrame()
        {
            currentFrame++;
            if (currentFrame == frames.Length) { currentFrame = 0; }
            gameObject.SetTexture(new ObjectTexture(frames[currentFrame], gameObject.Texture.GetFlipped(), gameObject.Texture.GetColor(), gameObject.Texture.GetRotation()));
        }
    }
}
