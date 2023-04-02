using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Utils.BeatReadr
{
    public struct BeatEvent
    {

        public int BPM;
        public int Beat;
        
        public BeatEvent(int bpm, int beat)
        {
            BPM = bpm;
            Beat = beat;
        }
    }

    public class Beatmap
    {
        /// <summary>
        /// Array of what beats the BPM is changed on.
        /// </summary>
        public BeatEvent[] BPM;
        public int Beat = 0;

        public int Length;

        public Beatmap(BeatEvent[] bpm, int length)
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
