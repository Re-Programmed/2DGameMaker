using _2DGameMaker.GAME_NAME.PLAYER_NAME;
using _2DGameMaker.GAME_NAME.PLAYER_NAME.Interactions;
using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Scripting;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.GAME_NAME.StageElements.Walls
{
    public class Ladder : PlayerInteractable
    {
        readonly bool ladderCap = false;

        public Ladder(GameObject gameObject, string arg0)
            :base(gameObject, ControlsManager.GetKey(ControlsManager.ControlOption.PlayerMove_UP), arg0.ToLower() == "true")
        {
            ladderCap = arg0.ToLower() == "true";
        }

        public override void OnInteract(PlayerName player)
        {
            player.SetClimbing(true, ladderCap);
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
