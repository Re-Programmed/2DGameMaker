using _2DGameMaker.Game.Stages;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.GAME_NAME
{
    public class GameName : Game.Game
    {
        public GameName()
            : base("Game Title")
        { }

        protected override void Close()
        {

        }

        protected override void Init()
        {
            StageManager.GenerateStage("ar1");
        }

        protected override void LateUpdate()
        {
            Utils.PhysX.PhysicsManager.PhysicsTick();
        }

        protected override void Render()
        {

        }
    }
}
