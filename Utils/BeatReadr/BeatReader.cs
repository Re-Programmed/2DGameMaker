using _2DGameMaker.Utils.AssetManagment;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Utils.BeatReadr
{
    public static class BeatReader
    {
        public delegate void BeatTick(Beatmap map);
        /// <summary>
        /// Every time a beat occurs in the music.
        /// </summary>
        public static BeatTick Tick;

        private static Beatmap currentMap;

        private static float fractionalBeat = 0;

        public static Dictionary<string, Beatmap> Beatmaps { get; private set; } = new Dictionary<string, Beatmap>();

        public static bool AddBeatmap(string name, Beatmap map)
        {
            if (!Beatmaps.ContainsKey(name))
            {
                Beatmaps.Add(name, map);
                Console.WriteLine("ADDED MAP: " + name);
                return true;
            }

            return false;
        }

        public static bool AddBeatmap(string name, string xmlData)
        {
            return AddBeatmap(name, XMLManager.LoadFromXMLString<Beatmap>(xmlData));
        }

        public static void Update()
        {
            if (currentMap == null) { return; }
            fractionalBeat += Game.GameTime.DeltaTimeScale();

            if (fractionalBeat >= 1f)
            {
                currentMap.Beat += 1;
                fractionalBeat = 0;
                Tick?.Invoke(currentMap);
            }
        }

        public static void SetCurrentMap(string name)
        {
            if(Beatmaps.ContainsKey(name))
            {
                currentMap = Beatmaps[name];
            }
        }
    }
}
