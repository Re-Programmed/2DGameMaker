using _2DGameMaker.DiscordRPC;
using DiscordRPC;
using _2DGameMaker.Rendering.Display;
using System;
using System.Collections.Generic;
using System.Text;
using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Stationaries;
using _2DGameMaker.Utils.Math;
using _2DGameMaker.Utils.AssetManagment;

namespace _2DGameMaker.Game
{
    class TemplateGame : Game
    {
        private Random randomBG = new Random();

        public TemplateGame(string title)
            :base(title)
        {

        }

        protected override void Close()
        {
            RPCManager.Dispose();
        }

        StaticObject[] test_object = new StaticObject[3];

        protected override void Init()
        {
            SetClearColor(new Utils.Math.Vec4((float)randomBG.NextDouble(), (float)randomBG.NextDouble(), (float)randomBG.NextDouble(), 0));

            RPCManager.Initialize("995784691179331584", new RichPresence()
            {
                Timestamps = new Timestamps(DateTime.UtcNow),
                Details = "Testing.",
                State = "Test.",
                Assets = new Assets()
                {
                    LargeImageKey = "testing",
                    LargeImageText = "Test.",
                    SmallImageKey = "testing"
                }
            });

            test_object[0] = new StaticObject(Vec2.Zero, new ObjectTexture(AssetManager.GetTexture("test_sp", "player")));
            test_object[1] = new StaticObject(Vec2.Zero, new ObjectTexture(AssetManager.GetTexture("test_image", "player")));
            test_object[2] = new StaticObject(Vec2.Zero, new ObjectTexture(AssetManager.GetTexture("test_kai", "player")));

            Instantiate(test_object[0], 1);
            Instantiate(test_object[1], 1);
            Instantiate(test_object[2], 1);
        }

        protected override void LateUpdate()
        {

            
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Render()
        {

        }

        float i = 0;
        protected override void Update()
        {
            base.Update();

            test_object[0].Rotate(1f * GameTime.DeltaTimeScale());
            test_object[1].Rotate(2f * GameTime.DeltaTimeScale());
            test_object[2].Rotate(3f * GameTime.DeltaTimeScale());

        }
    }
}
