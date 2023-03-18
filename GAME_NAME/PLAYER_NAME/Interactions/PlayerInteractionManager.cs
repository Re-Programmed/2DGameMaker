using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.GAME_NAME.PLAYER_NAME.Interactions
{
    public class PlayerInteractable 
    {
        public delegate void Interaction();
        public Interaction OnInteract;


    }

    public static class PlayerInteractionManager
    {
        static float checkRadius = 0f;

        public static void Interaction()
        {
            if (checkRadius == 0) { return; }

        }
    }
}
