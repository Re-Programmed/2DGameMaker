using _2DGameMaker.Game;
using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Scripting;
using _2DGameMaker.Utils.PhysX.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.GAME_NAME.PLAYER_NAME
{
    public class PlayerName : ObjectAppendedScript
    {

        private static float SPEED = 50f;

        public PlayerName(GameObject gameObject, string arg0)
            : base(gameObject)
        {

        }

        protected override void destroy()
        {

        }

        DynamicCollisionObject collider;

        protected override void init()
        {
            gameObject.AppendScript(new DynamicCollisionObject(gameObject, null));
        }

        float G = 0f;
        protected override void update()
        {
            G += 0.3f;
            gameObject.GetPosition().AddY(G * GameTime.DeltaTimeScale());
            updateMovement();
        }

        private void updateMovement()
        {
            if (Input.Input.GetKey(ControlsManager.GetKey(ControlsManager.ControlOption.PlayerMove_LEFT)))
            {
                gameObject.Translate(Utils.Math.Vec2.OneX * -SPEED * GameTime.DeltaTimeScale());
            }

            if (Input.Input.GetKey(ControlsManager.GetKey(ControlsManager.ControlOption.PlayerMove_RIGHT)))
            {
                gameObject.Translate(Utils.Math.Vec2.OneX * SPEED * GameTime.DeltaTimeScale());
            }

        }
    }
}
