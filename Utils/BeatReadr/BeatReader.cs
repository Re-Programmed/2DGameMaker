using _2DGameMaker.Utils.AssetManagment;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Utils.BeatReadr
{
    public static class BeatReader
    {
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
    }
}
