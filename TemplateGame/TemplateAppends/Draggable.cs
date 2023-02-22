using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Collisions;
using _2DGameMaker.Objects.Scripting;
using _2DGameMaker.Utils.AssetManagment;
using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.TemplateGame.TemplateAppends
{
    public class Draggable : ObjectAppendedScript
    {

        bool dragging = false;

        public Draggable(GameObject gameObject, string saveID)
            : base(gameObject)
        {
            
        }

        protected override void destroy()
        {
            
        }

        protected override void init()
        {

        }

        protected override void update()
        {
            if (dragging)
            {
                Vec2 mPos = Game.Game.INSTANCE.GetCamera().MouseToWorldCoords(Input.Input.GetMousePosition());
                gameObject.SetPosition(mPos - gameObject.GetScale() / 2f);
            }

            int mEvent = Input.Input.GetMouseButtonEvent(GLFW.MouseButton.Left);

            if (mEvent == Input.Input.MOUSE_PRESSED)
            {
                Vec2 mPos = Game.Game.INSTANCE.GetCamera().MouseToWorldCoords(Input.Input.GetMousePosition());
                if (CollisionCheck.BoxCheck(mPos - gameObject.GetScale() / 2f, gameObject.GetPosition(), gameObject.GetScale()))
                {
                    dragging = true;
                }
            }else if (mEvent == Input.Input.MOUSE_RELEASED)
            {
                dragging = false;
            }
        }
    }
}
