using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Scripting;
using _2DGameMaker.Utils.BeatReadr;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.GAME_NAME.BeatSync
{
    public abstract class BeatUpdatedObject : ObjectAppendedScript
    {
        public BeatUpdatedObject(GameObject gameObject)
            : base(gameObject)
        {

        }

        /// <summary>
        /// If the OnBeat callback is added to the Tick delegate.
        /// </summary>
        bool btAdded = false;
        public override void OnDisable()
        {
            if (btAdded == false) { return; }
            BeatReader.Tick -= OnBeat;
            btAdded = false;
        }

        public override void OnLoad()
        {
            if (btAdded == true) { return; }
            BeatReader.Tick += OnBeat;
            btAdded = true;
        }

        /// <summary>
        /// Called on a beat in the current song.
        /// </summary>
        public abstract void OnBeat(Beatmap map);
    }
}
