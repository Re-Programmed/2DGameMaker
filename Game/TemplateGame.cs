using _2DGameMaker.DiscordRPC;
using DiscordRPC;
using _2DGameMaker.Rendering.Display;
using System;
using System.Collections.Generic;
using System.Text;

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

        protected override void Init()
        {
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
        }

        protected override void LateUpdate()
        {

        }

        protected override void LoadContent()
        {

        }

        protected override void Render()
        {

        }

        float i = 0;
        protected override void Update()
        {
            i += GameTime.DeltaTimeScale();
            if(i > 0.5f)
            {
                SetClearColor(new Utils.Math.Vec4((float)randomBG.NextDouble(), (float)randomBG.NextDouble(), (float)randomBG.NextDouble(), 0));
                i = 0;
            }
        }
    }
}
