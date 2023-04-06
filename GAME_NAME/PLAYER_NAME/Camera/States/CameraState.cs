using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.GAME_NAME.PLAYER_NAME.Camera.States
{
    public abstract class CameraState
    {
        public unsafe abstract Vec2 PerformOperation(DynamicCamera camera, float* zoom);
    }
}
