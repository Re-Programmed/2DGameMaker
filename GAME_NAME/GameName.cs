using _2DGameMaker.Game.Stages;
using _2DGameMaker.GAME_NAME.GUI;
using _2DGameMaker.GAME_NAME.PLAYER_NAME;
using _2DGameMaker.GAME_NAME.PLAYER_NAME.Camera;
using _2DGameMaker.GAME_NAME.PLAYER_NAME.Interactions;
using _2DGameMaker.Objects.Stationaries;
using _2DGameMaker.Rendering.Display;
using _2DGameMaker.Utils.AssetManagment;
using _2DGameMaker.Utils.BeatReadr;
using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.GAME_NAME
{
    public class GameName : Game.Game
    {
        /// <summary>
        /// The value to multiply sprites scales by, and to lock sprites positions to, to emulate a 224x320 resolution.
        /// </summary>
        public const byte v32X_RETRO_SCALAR = 192;

        public static PLAYER_NAME.PlayerName ThePlayer;

        public GameName()
            : base("Game Title")
        { }

        protected override void loadCamera()
        {
            cam = new DynamicCamera(Vec2.Zero, 1f);
        }

        protected override void Close()
        {

        }

        protected override void Init()
        {
            LevelManager.Init();

            StageElements.Grass.Grass.InitilizeGrassSpawns();

            StageManager.GenerateStage("ar1");

            Beatmap bmp = new Beatmap(new BeatEvent[] { new BeatEvent(120, 0) }, 120 * 4);
            BeatReader.AddBeatmap("test_120bpm", bmp);
            BeatReader.SetCurrentMap("test_120bpm");

        }

        public static PlayerStats GetSavedPlayerStats()
        {
            PlayerStats stats;
            AppDataManager.GetFile("pstat", out stats);
            return stats;
        }

        protected override void Update()
        {
            base.Update();
            BeatReader.Update();
            PlayerInteractionManager.CheckInteraction();
        }

        protected override void LateUpdate()
        {
            _2DGameMaker.Utils.PhysX.PhysicsManager.PhysicsTick();
        }

        protected override void Render()
        {

        }
    }
}
