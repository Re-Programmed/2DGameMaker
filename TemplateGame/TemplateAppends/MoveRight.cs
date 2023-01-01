using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Scripting;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.TemplateGame.TemplateAppends
{
    public class MoveRight : ObjectAppendedScript
    {
        float amount;

        public MoveRight(GameObject gameObject, string arg0)
            : base(gameObject)
        {
             amount = float.Parse(arg0);
        }

        protected override void destroy()
        {

        }

        protected override void init()
        {

        }

        protected override void update()
        {
            //Console.WriteLine(amount);
            gameObject.Translate(Utils.Math.Vec2.OneX * amount);
        }
    }
}
