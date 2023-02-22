using _2DGameMaker.Utils.AssetManagment;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Utils.BeatReadr
{
    public static class BeatReader
    {
        public static void Init()
        {
            Console.WriteLine(XMLManager.ToXML(new Beatmap(new Dictionary<int, int>() { { 0, 120 }, { 25, 240 } }, 50)));
        }
    }
}
