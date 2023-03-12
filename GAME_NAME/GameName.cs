using _2DGameMaker.Game.Stages;
using _2DGameMaker.GAME_NAME.GUI;
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

        bool g = false;
        protected override void Update()
        {
            base.Update();
            if(Input.Input.GetKey(GLFW.Keys.G) && !g)
            {
                g = true;
                TransitionEffects.MultiCircleEffect(true);
            }
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
