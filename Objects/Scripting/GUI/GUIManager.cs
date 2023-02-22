using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Objects.Scripting.GUI
{
    public static class GUIManager
    {
        public delegate void GUIEvent(GUIEventType type, string code);
        public static GUIEvent GUIEventHandler;

        public enum GUIEventType
        { 
            ButtonPress
        }
    }
}
