using _2DGameMaker.Game;
using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Scripting;
using _2DGameMaker.Objects.Scripting.GUI;
using _2DGameMaker.Objects.Stationaries;
using _2DGameMaker.Rendering.Display;
using _2DGameMaker.Utils.AssetManagment;
using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.StageCreator
{
    public static class ScriptManager
    {
        const GLFW.Keys OPEN_GUI_KEY = GLFW.Keys.T;

        static bool GUIopen;
        static StaticObject GUIMenu;

        static float openPosition, closedPosition;
        public static void CreateScriptGUI()
        {
            Vec2 sca = new Vec2(400f, DisplayManager.WindowSize.Y + 20f);
            StaticObject so = new StaticObject(new Vec2(DisplayManager.WindowSize.X / 2 - 400f, -DisplayManager.WindowSize.Y / 2), sca, 0f, new ObjectTexture(AssetManager.GetTexture("black_0.5", "general_assets")), true, null, true);

            openPosition = so.GetPosition().X;
            closedPosition = so.GetPosition().X + 500f;

            Game.Game.INSTANCE.Instantiate(so, 2);

            createPlaceButtons(so);

            GUIMenu = so;
        }

        static readonly string[,] objectAppenedScripts = new string[,] {

             { "_2DGameMaker.Utils.PhysX.Components.DynamicCollisionObject:true", "component_dynamic_collider" },
             { "_2DGameMaker.Utils.PhysX.Components.StaticCollisionObject:true", "component_collider" },
             { "_2DGameMaker.GAME_NAME.StageElements.Grass.Grass:null", "component_grass" },
             { "_2DGameMaker.Utils.PhysX.Components.GravityModifier:0.07:0.17", "component_physics" },
             { "_2DGameMaker.GAME_NAME.StageElements.Walls.Ladder:null", "component_ladder" },
             { "_2DGameMaker.GAME_NAME.StageElements.Walls.Ladder:true", "component_ladder_top" }
        };

        public static string GetOASParams(ObjectAppendedScript oas, out string[] param)
        {
            for (int i = 0; i < objectAppenedScripts.Length / 2; i++)
            {
                string[] dat = objectAppenedScripts[i, 0].Split(":");
                if (oas.GetType().ToString() == dat[0])
                {
                    param = new string[dat.Length - 1];
                    for (int x = 0; x < dat.Length; x++)
                    {
                        if (x != 0) { param[x - 1] = dat[x]; }
                    }

                    return dat[0];
                }
            }

            param = null;
            return "";
        }

        static Dictionary<StaticObject, float> placeButtons;

        private static void createPlaceButtons(GameObject parent)
        {
            placeButtons = new Dictionary<StaticObject, float>();

            for (int i = 0; i < objectAppenedScripts.Length / 2; i++)
            {
                Console.WriteLine("ADDED ICON");
                StaticObject button = new StaticObject(new Vec2(i % 2 == 0 ? parent.GetScale().X / 2 : 0, MathF.Floor(i / 2) * parent.GetScale().Y * 0.18f), new Vec2(0.5f, 0.18f), 0f, new ObjectTexture(AssetManager.GetTexture(objectAppenedScripts[i, 1], "ui_stage_creator")), true, parent, true);
                button.AppendScript(new Button(button, StageCreator.SCRIPT_BUTTON_PREFIX + ":" + objectAppenedScripts[i, 0]));

                placeButtons.Add(button, button.GetLocalPosition().Y);

                Game.Game.INSTANCE.Instantiate(button, 2);

            }

        }

        static bool keyTdown = false;
        public static void Update()
        {
            openCloseGUI();

            if (GUIopen)
            {
                GUIMenu.GetPosition().SetX(Utils.Math.Math.Lerp(GUIMenu.GetPosition().X, openPosition, GameTime.DeltaTimeScale() * 35f));
            }
            else
            {
                GUIMenu.GetPosition().SetX(Utils.Math.Math.Lerp(GUIMenu.GetPosition().X, closedPosition, GameTime.DeltaTimeScale() * 35f));
            }
        }

        static void openCloseGUI()
        {
            if (Input.Input.GetKey(OPEN_GUI_KEY))
            {
                if (!keyTdown)
                {
                    keyTdown = true;

                    GUIopen = !GUIopen;
                }

                return;
            }

            keyTdown = false;
        }
    }
}
