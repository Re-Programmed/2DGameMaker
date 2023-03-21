using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Game
{
    static class GameTime
    {
        private const float NORM_CONSTANT = 0.00027203110021294f;

        public static float DeltaTime { get; set; }

        public static float TimeScale { get; set; } = 1f;

        public static float DeltaTimeScale() { return DeltaTime * TimeScale; }

        public static float NormalizedDeltaTime() { return DeltaTime * TimeScale / NORM_CONSTANT; }

        public static float TotalElapsedSeconds { get; set; }
    }
}
