using _2DGameMaker.Utils.AssetManagment;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.GAME_NAME
{
    public static class ControlsManager
    {
        private static Dictionary<ControlOption, GLFW.Keys> keyRef = new Dictionary<ControlOption, GLFW.Keys>();
        public enum ControlOption
        { 
            PlayerMove_UP,
            PlayerMove_DOWN,
            PlayerMove_LEFT,
            PlayerMove_RIGHT,
            PlayerMove_JUMP
        }
        
        /// <summary>
        /// Returns the GLFW.Key value of the ControlOption specified.
        /// </summary>
        public static GLFW.Keys GetKey(ControlOption option)
        {
            return keyRef[option];
        }

        /// <summary>
        /// Adds a new key or changes an existing one.
        /// </summary>
        public static void UpdateKey(ControlOption option, GLFW.Keys key)
        {
            if (keyRef.ContainsKey(option))
            {
                keyRef[option] = key;
                return;
            }

            keyRef.Add(option, key);
        }

        private static void saveKeys()
        {
            int[] options = new int[keyRef.Count];
            int[] inputs = new int[keyRef.Count];

            int i = 0;
            foreach(KeyValuePair<ControlOption, GLFW.Keys> kvp in keyRef)
            {
                options[i] = (int)kvp.Key;
                inputs[i] = (int)kvp.Value; 
                i++;
            }

            int[][] sav = new int[][] { options, inputs };

            AppDataManager.UpdateFile("input\\controls", sav);
        }

        /// <summary>
        /// Sets the default keybinds. (POSSIBLY REMOVE LATER)
        /// </summary>
        public static void RegisterDefaults()
        {
            UpdateKey(ControlOption.PlayerMove_UP, GLFW.Keys.W);
            UpdateKey(ControlOption.PlayerMove_DOWN, GLFW.Keys.S);
            UpdateKey(ControlOption.PlayerMove_LEFT, GLFW.Keys.A);
            UpdateKey(ControlOption.PlayerMove_RIGHT, GLFW.Keys.D);
            UpdateKey(ControlOption.PlayerMove_JUMP, GLFW.Keys.Space);

            saveKeys();
        }

        public static void LoadKeysFromSave()
        {
            int[][] loadedControls;
            if(AppDataManager.GetFile<int[][]>("input\\controls", out loadedControls))
            {
                for(int i = 0; i < loadedControls[0].Length; i++)
                {
                    UpdateKey((ControlOption)loadedControls[0][i], (GLFW.Keys)loadedControls[1][i]);
                }
            }
            else
            {
                RegisterDefaults();
            }
        }
    }
}
