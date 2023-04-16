using _2DGameMaker.Game;
using _2DGameMaker.Objects;
using _2DGameMaker.Rendering.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Utils.Animatr
{
    public class TimeRelativeAnimation : Animation
    {
        readonly float speed;
        float T;

        public TimeRelativeAnimation(GameObject gameObject, float speed, Texture2D[] textures)
            : base(gameObject, textures)
        {
            this.speed = speed;
        }

        public TimeRelativeAnimation(GameObject gameObject, string speed, string[,] textures)
            : base(gameObject, AnimationBuilder.BuildT2D(textures))
        {
            this.speed = float.Parse(speed); 
        }

        protected override void destroy()
        {
            
        }

        protected override void init()
        {

        }

        protected override void update()
        {
            if (!playing) { return; }
            T += GameTime.NormalizedDeltaTime();

            if(T >= speed)
            {
                T = 0;
                advanceFrame();
            }
        }
    }
}
