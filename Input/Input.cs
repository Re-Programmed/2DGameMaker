using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using _2DGameMaker.Rendering.Display;
using GLFW;
using static _2DGameMaker.OpenGL.GL;

#pragma warning disable

namespace _2DGameMaker.Input
{
    static class Input
    {
        public const int MOUSE_PRESSED = 1, MOUSE_NO_CHANGE = 0, MOUSE_RELEASED = 2;

        public delegate void ScrollCallback(Window window, double xoff, double yoff);
        public static ScrollCallback OnScroll;

        private static List<MouseButton> pressedButtons = new List<MouseButton>();

        public static bool GetKey(Keys key)
        {
            return (Glfw.GetKey(DisplayManager.Window, key) == InputState.Press);
        }

        public static int GetNumberKey()
        {
            if (GetKey(Keys.Alpha0)) { return 0; } else
            if (GetKey(Keys.Alpha1)) { return 1; } else
            if (GetKey(Keys.Alpha2)) { return 2; } else
            if (GetKey(Keys.Alpha3)) { return 3; } else
            if (GetKey(Keys.Alpha4)) { return 4; } else
            if (GetKey(Keys.Alpha5)) { return 5; } else
            if (GetKey(Keys.Alpha6)) { return 6; } else
            if (GetKey(Keys.Alpha7)) { return 7; } else
            if (GetKey(Keys.Alpha8)) { return 8; } else
            if (GetKey(Keys.Alpha9)) { return 9; } else
            if (GetKey(Keys.Minus)) { return -2; }

            return -1;
        }

        public static Vector2 GetMousePosition()
        {
            double x = -1, y = -1;
            Glfw.GetCursorPosition(DisplayManager.Window, out x, out y);

            return new Vector2((float)x, (float)y);
        }

        public static bool GetMouseButton(MouseButton mb)
        {
            return (Glfw.GetMouseButton(DisplayManager.Window, mb) == InputState.Press);
        }

        /// <summary>
        /// Returns if a mouse button was pressed or released in a given frame.
        /// </summary>
        /// <param name="mb">Button to get.</param>
        /// <returns>If a mouse button was pressed or released in a given frame.</returns>
        public static int GetMouseButtonEvent(MouseButton mb)
        {
            bool pressed = (Glfw.GetMouseButton(DisplayManager.Window, mb) == InputState.Press);

            if (pressedButtons.Contains(mb))
            {
                if (pressed) { return MOUSE_NO_CHANGE; }
                pressedButtons.Remove(mb);
                return MOUSE_RELEASED;
            }
            else
            {
                if (!pressed) { return MOUSE_NO_CHANGE; }
                pressedButtons.Add(mb);
                return MOUSE_PRESSED;
            }

            
        }

        public static float[] GetJoystick(Joystick joystick)
        {
            if(Glfw.JoystickPresent(joystick))
            {
                return Glfw.GetJoystickAxes(joystick);
            }

            return null;
        }

        public static InputState[] GetJoystickButtons(Joystick joystick)
        {
            if (Glfw.JoystickPresent(joystick))
            {
                return Glfw.GetJoystickButtons(joystick);
            }

            return null;
        }

        /// <summary>
        /// Inits callbacks.
        /// </summary>
        public static void Init()
        {
            Glfw.SetScrollCallback(DisplayManager.Window, (Window w, double offX, double offY) => { OnScroll?.Invoke(w, offX, offY); });
        }
    }
}
