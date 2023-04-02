using _2DGameMaker.Objects;
using _2DGameMaker.Utils.BeatReadr;
using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.GAME_NAME.BeatSync
{
    public class PulseObject : BeatUpdatedObject
    {
        bool colored = false;
        readonly Vec4 color;
        readonly Vec4 initColor;
        public PulseObject(GameObject gameObject, string color)
            : base(gameObject)
        {
            initColor = gameObject.Texture.GetColor();
            this.color = Vec4.StringToColor(color);
        }

        public override void OnBeat(Beatmap map)
        {
            if(colored)
            {
                Console.WriteLine("NORMAL");
                gameObject.Texture.SetColor(initColor);
                colored = false;
                return;
            }

            Console.WriteLine("PULSED");
            gameObject.Texture.SetColor(color);
            colored = true;
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
    }
}
