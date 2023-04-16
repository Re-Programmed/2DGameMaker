using _2DGameMaker.Objects.Stationaries;
using _2DGameMaker.Rendering.Display;
using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.GAME_NAME.GUI.GameUI
{
    public static class StatsDisplay
    {
        const byte STATS_UI_LAYER = 3;
        const byte SCORE_ZERO_PADDING = 6;

        public static void Initilize()
        {
            UpdateScore(0);
        }

        static UINumberObject[] numberObjects;
        public static void UpdateScore(int score)
        {
            if(numberObjects != null)
            {
                foreach(UINumberObject numberObject in numberObjects)
                {
                    Game.Game.INSTANCE.Destroy(numberObject, STATS_UI_LAYER);
                }
            }

            numberObjects = new UINumberObject[SCORE_ZERO_PADDING + 1];
            UINumberObject[] objects;
            Vec2 pos = new Vec2(-DisplayManager.WindowSize.X / 2, -DisplayManager.WindowSize.Y / 2);
            numberObjects[0] = NumberRenderer.GetNumber(score, pos, 64f, out objects, STATS_UI_LAYER, 6);

            for (int i = 0; i < objects.Length; i++) { numberObjects[i + 1] = objects[i]; }

            Console.WriteLine(numberObjects[1].GetPosition());

            Game.Game.INSTANCE.Instantiate(numberObjects[0], STATS_UI_LAYER);
        }
    }
}
