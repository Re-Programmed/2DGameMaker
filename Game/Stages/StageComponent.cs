using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Game.Stages
{
    public struct StageComponent
    {
        public string Name;
        public string[] Args;

        public StageComponent(string name, string[] args)
        {
            Name = name;
            Args = args;
        }
    }
}
