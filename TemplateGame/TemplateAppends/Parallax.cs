using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Scripting;
using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.TemplateGame.TemplateAppends
{
    public class Parallax : ObjectAppendedScript
    {
        Vec2 initPosition;
        float parallaxAmount;

        public Parallax(GameObject gameObject, string arg0)
            : base(gameObject)
        {
            parallaxAmount = float.Parse(arg0);
        }

        protected override void destroy()
        {

        }

        protected override void init()
        {
            initPosition = gameObject.GetLocalPosition();
        }

        protected override void update()
        {
            gameObject.SetPosition(initPosition / parallaxAmount * Game.Game.INSTANCE.GetCamera().FocusPosition);
        }
    }
}
