using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Sound
{
    public struct SoundLibrary
    {
        public readonly string LibraryName;
        public Dictionary<string, SoundClip> Sounds { get; private set; }

        public double Volume { get; private set; }

        public void SetVolume(double volume)
        {
            Volume = volume;

            foreach(SoundClip sc in Sounds.Values)
            {
                sc.UpdateVolume();
            }
        }

        public SoundLibrary(string libraryName)
        {
            LibraryName = libraryName;
            Volume = 1;
            Sounds = new Dictionary<string, SoundClip>();
        }

        internal void Add(string name, SoundClip soundClip)
        {
            Sounds.Add(name, soundClip);
        }

        internal void Add(string name, string file)
        {
            Sounds.Add(name, new SoundClip(file, this));
        }
    }
}
