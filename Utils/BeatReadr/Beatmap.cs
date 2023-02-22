using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Utils.BeatReadr
{
    public class Beatmap
    {
        /// <summary>
        /// Dictionary of what beats the BPM is changed on.
        /// </summary>
        private Dictionary<int, int> BPM;
        private int Beat = 0;

        private int Length;

        public Beatmap(Dictionary<int, int> bpm, int length)
        {
            BPM = bpm;
            Length = length;
        }

        /// <summary>
        /// Paramaterless constructor for serialization.
        /// </summary>
        public Beatmap()
        {

        }
    }
}
