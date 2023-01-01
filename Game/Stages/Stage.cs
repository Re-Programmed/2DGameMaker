using System;
using System.Collections.Generic;
using _2DGameMaker.Objects;

namespace _2DGameMaker.Game.Stages
{
    public struct Stage
    {
        public StageObject[] objects;

        public Stage(StageObject[] objects)
        {
            this.objects = objects;
        }
    }
}
