using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace _2DGameMaker.Sound
{
    public static class SoundManager
    {
        const string SoundDIR = ".\\Assets\\Sound";

        static List<SoundLibrary> sounds = new List<SoundLibrary>();

        private static bool containsLibrary(string lib)
        {
            foreach(SoundLibrary sl in sounds)
            {
                if(sl.LibraryName == lib) { return true; }
            }

            return false;
        }

        private static SoundLibrary? getLibrary(string lib)
        {
            foreach (SoundLibrary sl in sounds)
            {
                return sl;
            }

            return null;
        }

        private static void registerSoundLibrary(string name)
        {
            sounds.Add(new SoundLibrary(name));
        }

        private static void registerSound(string file)
        {
            string name = Path.GetFileNameWithoutExtension(file);

            string library = Path.GetFileName(Path.GetDirectoryName(file));

            if(!containsLibrary(library)) { registerSoundLibrary(library); }

            getLibrary(library)?.Add(name, file);
        }

        public static SoundClip GetSound(string library, string name)
        {
            return getLibrary(library)?.Sounds[name];
        }

        public static void LoadSounds()
        {
            foreach(string library in Directory.GetDirectories(SoundDIR))
            {
                foreach(string file in Directory.GetFiles(library))
                {
                    registerSound(file);
                }
            }
        }
    }
}
