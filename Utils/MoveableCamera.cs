using System;
using System.Collections.Generic;
using System.Text;
using _2DGameMaker.GAME_NAME;
using _2DGameMaker.Rendering.Cameras;
using _2DGameMaker.Utils.Math;

namespace _2DGameMaker.Utils
{
    public class MoveableCamera : Camera2d
    {
        public MoveableCamera(Vec2 focusPosition, float zoom)
            : base(focusPosition, zoom)
        {
            Game.Game.INSTANCE.UpdateE += update;
            Input.Input.OnScroll += onScroll;
        }

        ~MoveableCamera()
        {
            Game.Game.INSTANCE.UpdateE -= update;
            Input.Input.OnScroll -= onScroll;
        }

        private void update()
        {
            if (Input.Input.GetKey(ControlsManager.GetKey(ControlsManager.ControlOption.PlayerMove_UP)))
            {
                FocusPosition -= Vec2.OneY;
            }

            if (Input.Input.GetKey(ControlsManager.GetKey(ControlsManager.ControlOption.PlayerMove_DOWN)))
            {
                FocusPosition += Vec2.OneY;
            }

            if (Input.Input.GetKey(ControlsManager.GetKey(ControlsManager.ControlOption.PlayerMove_LEFT)))
            {
                FocusPosition -= Vec2.OneX;
            }

            if (Input.Input.GetKey(ControlsManager.GetKey(ControlsManager.ControlOption.PlayerMove_RIGHT)))
            {
                FocusPosition += Vec2.OneX;
            }
        }

        private void onScroll(GLFW.Window w, double x, double y)
        {
            if (Input.Input.GetKey(GLFW.Keys.LeftControl)) { return; }
          
            Zoom += (float)y / 100f;
        }

    }
}
