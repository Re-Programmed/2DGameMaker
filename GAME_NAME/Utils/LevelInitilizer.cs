using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Scripting;
using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.GAME_NAME.Utils
{
    public class LevelInitilizer : ObjectAppendedScript
    {

        public LevelInitilizer(GameObject gameObject, string arg0)
            : base(gameObject)
        {
            Vec4 levelColor = Vec4.StringToColor(arg0);
            Game.Game.INSTANCE.SetClearColor(levelColor);
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
