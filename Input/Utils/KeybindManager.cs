using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Input.Utils
{
    static class KeybindManager
    {
        const string KeysFilePath = "prefs/keys.b64";

        static Dictionary<string, Keybind> keybinds;

        public static bool GetKeybind(string bind)
        {
            if(!keybinds.ContainsKey(bind))
            {
                return false;
            }

            return keybinds[bind].GetDown();
        }

        public static void UpdateKeybind(string bind, GLFW.Keys key)
        {
            keybinds[bind] = new Keybind(key);
        }

        public static void AddKeybind(string bind, GLFW.Keys key)
        {
            keybinds.Add(bind, new Keybind(key));
        }
    }
}
