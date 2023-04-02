using _2DGameMaker.GAME_NAME.PLAYER_NAME.Interactions;
using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Scripting;
using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.GAME_NAME.StageElements.Walls
{
    public class FrontWall : ObjectAppendedScript
    {
        const uint TOLERENCE = 0x12C;
        public FrontWall(GameObject gameObject, string arg0)
            : base(gameObject)
        {

        }
        protected override void destroy()
        {

        }

        protected override void init()
        {
            /*
            char a = 'A';
            var b = a >> 1;

            Console.WriteLine((char)b);
            */
        }

        bool colorSet = false;
        protected override void update()
        {
            if (PlayerInteractionManager.PlayerInBounds(gameObject.GetScale().X + TOLERENCE, gameObject.GetScale().Y + TOLERENCE, gameObject.GetPosition()))
            {
                if(!colorSet)
                {
                    colorSet = true;
                    gameObject.SetEnabledOnLoad(false);
                }
            }
            else
            {
                colorSet = false;
                gameObject.SetEnabledOnLoad(true);
            }
        }
    }
}
