using _2DGameMaker.Game.Stages;
using _2DGameMaker.GAME_NAME.GUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.GAME_NAME
{
    internal static class LevelManager
    {
        internal static void Init()
        {
            StageManager.OnStageGenerated += onStageGenerated;
        }

        /// <summary>
        /// Called when the game loads a new stage.
        /// </summary>
        static void onStageGenerated()
        {
            TransitionEffects.MultiCircleEffect(true);
        }
    }
}
