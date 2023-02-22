using _2DGameMaker.Objects.Collisions;
using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Objects.Scripting.GUI
{
    public class Button : ObjectAppendedScript
    {
        public Button(GameObject gameObject, string buttonCode) : base(gameObject)
        {
            this.buttonCode = buttonCode;
        }

        private readonly string buttonCode;

        protected override void destroy()
        {

        }

        protected override void init()
        {

        }

        protected override void update()
        {
            Vec2 mouse = Game.Game.INSTANCE.CurrMousePositionWorldCoords;
            if (CollisionCheck.BoxCheck(gameObject.IsUI() ? mouse - Game.Game.INSTANCE.GetCamera().FocusPosition : mouse, gameObject))
            {
                if (Input.Input.GetMouseButtonEvent(GLFW.MouseButton.Left) == Input.Input.MOUSE_PRESSED)
                {
                    GUIManager.GUIEventHandler?.Invoke(GUIManager.GUIEventType.ButtonPress, buttonCode);
                }
            }
        }
    }
}
