using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Media;

namespace _2DGameMaker.Sound
{
    public class SoundClip : MediaPlayer
    {
        SoundLibrary slRef;

        double iVolume = 1;

        public SoundClip(string file, SoundLibrary slRef)
        {
            Open(new Uri(Path.GetFullPath(file)));
            this.slRef = slRef;
            UpdateVolume();
        }

        public void SetVolume(double volume)
        {
            iVolume = volume;
            UpdateVolume();
        }

        internal void UpdateVolume()
        {
            Volume = iVolume * slRef.Volume;
        }
    }
}
