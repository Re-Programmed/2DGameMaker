using System;
using System.Collections.Generic;
using System.Text;
using _2DGameMaker.Rendering.Cameras;
using _2DGameMaker.Utils.Math;

namespace _2DGameMaker.TemplateGame
{
    public class MoveableCamera : Camera2d
    {
        public MoveableCamera(Vec2 focusPosition, float zoom)
            :base(focusPosition, zoom)
        {
            TemplateGame.INSTANCE.UpdateE += update;
            Input.Input.OnScroll += onScroll;
        }
        
        ~MoveableCamera()
        {
            TemplateGame.INSTANCE.UpdateE -= update;
            Input.Input.OnScroll -= onScroll;
        }

        private void update()
        {
            if (Input.Input.GetKey(GLFW.Keys.W))
            {
                FocusPosition -= Vec2.OneY;
            }

            if (Input.Input.GetKey(GLFW.Keys.S))
            {
                FocusPosition += Vec2.OneY;
            }

            if (Input.Input.GetKey(GLFW.Keys.A))
            {
                FocusPosition -= Vec2.OneX;
            }

            if (Input.Input.GetKey(GLFW.Keys.D))
            {
                FocusPosition += Vec2.OneX;
            }
        }

        private void onScroll(GLFW.Window w, double x, double y)
        {
            Zoom += (float)y/100f;
        }

    }
}
