using GLFW;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Input.Utils
{
    struct Keybind
    {
        public Keys key { get; set; }

        public Keybind(Keys key)
        {
            this.key = key;
        }

        public bool GetDown()
        {
            return Input.GetKey(key);
        }
    }
}
