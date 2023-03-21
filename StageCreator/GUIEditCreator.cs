using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Scripting.GUI;
using _2DGameMaker.Objects.Stationaries;
using _2DGameMaker.Rendering.Display;
using _2DGameMaker.Rendering.Sprites;
using _2DGameMaker.Utils.AssetManagment;
using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.StageCreator
{
    public static class GUIEditCreator
    {
        public static StaticObject CreateSidebar()
        {
            Vec2 sca = new Vec2(400f, DisplayManager.WindowSize.Y + 20f);
            StaticObject so = new StaticObject(new Vec2(-DisplayManager.WindowSize.X/2, -DisplayManager.WindowSize.Y/2), sca, 0f, new ObjectTexture(AssetManager.GetTexture("black_0.5", "general_assets")), true, null, true);

            Game.Game.INSTANCE.Instantiate(so, 2);

            return so;
        }

        const byte SCROLL_MULTIPLIER = 10;
        public static void SetPlaceButtonOffset(float y)
        {
            y *= SCROLL_MULTIPLIER;
            foreach(KeyValuePair<StaticObject, float> b in placeButtons)
            {
                b.Key.SetPosition(new Vec2(b.Key.GetLocalPosition().X, b.Value + y));
            }
        }

        static Dictionary<StaticObject, float> placeButtons;

        public static void CreatePlaceButtons(GameObject parent)
        {
            placeButtons = new Dictionary<StaticObject, float>();
            int i = 0;
            foreach (KeyValuePair<string, Dictionary<string, Texture2D>> library in AssetManager.Textures)
            {
                foreach (KeyValuePair<string, Texture2D> texture in library.Value)
                {
                    StaticObject button = new StaticObject(new Vec2(i % 2 == 0 ? parent.GetScale().X / 2 : 0, MathF.Floor(i / 2) * parent.GetScale().Y * 0.18f), new Vec2(0.5f, 0.18f), 0f, new ObjectTexture(AssetManager.GetTexture(texture.Key, library.Key)), true, parent, true);
                    button.AppendScript(new Button(button, "placeobj:" + texture.Key + ":" + library.Key));

                    placeButtons.Add(button, button.GetLocalPosition().Y);

                    Game.Game.INSTANCE.Instantiate(button, 2);
                    i++;
                }
            }

        }

        public static void InitGUI()
        {
            Game.Game.INSTANCE.SetClearColor(new Vec4(1, 0, 0, 1));

            CreatePlaceButtons(CreateSidebar());
        }
    }
}
