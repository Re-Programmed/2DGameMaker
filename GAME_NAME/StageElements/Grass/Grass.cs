using _2DGameMaker.Game.Stages;
using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Scripting;
using _2DGameMaker.Objects.Stationaries;
using _2DGameMaker.Utils.AssetManagment;
using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.GAME_NAME.StageElements.Grass
{
    public class Grass : ObjectAppendedScript
    {
        private const int GRASS_SPAWN_CHANCE = 5;

        private static GameObject[] grassSpawns;
        private static Random grassRandom = new Random();

        public Grass(GameObject gameObject, string arg0)
           : base(gameObject)
        {

        }
        protected override void destroy()
        {

        }

        protected override void init()
        {
            for (int x = 0; x < 32; x++)
            {
                if(grassRandom.Next(101) < GRASS_SPAWN_CHANCE)
                {
                    int r = grassRandom.Next(grassSpawns.Length);
                    StaticObject newSpawn = grassSpawns[r].Clone<StaticObject>();
                    newSpawn.SetPosition(gameObject.GetPosition() + Vec2.OneX * x - Vec2.OneY * newSpawn.GetScale().Y);
                    Game.Game.INSTANCE.Instantiate(newSpawn, 2);
                }
            }
        }

        protected override void update()
        {

        }

        public static void InitilizeGrassSpawns()
        {
            grassSpawns = new GameObject[1];
            /*flower_1*/grassSpawns[0] = new StaticObject(Vec2.Zero, new ObjectTexture(AssetManager.GetTexture("flower_1", "grass")));

            Console.WriteLine(grassSpawns);
        }
    }
}
