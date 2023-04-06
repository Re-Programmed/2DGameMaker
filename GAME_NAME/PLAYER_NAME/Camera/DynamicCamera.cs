using _2DGameMaker.GAME_NAME.PLAYER_NAME.Camera.States;
using _2DGameMaker.Rendering.Cameras;
using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.GAME_NAME.PLAYER_NAME.Camera
{
    /// <summary>
    /// A camera with multiple states and triggers.
    /// </summary>
    public class DynamicCamera : Camera2d
    {
        private CameraState currentState = new CamStateFollowPlayer(0.001f);

        public DynamicCamera(Vec2 focusPosition, float zoom)
            : base(focusPosition, zoom)
        {
            Game.Game.INSTANCE.UpdateE += update;
        }

        ~DynamicCamera()
        {
            Game.Game.INSTANCE.UpdateE -= update;
        }

        public void SetCameraState(CameraState state)
        {
            currentState = state;
        }

        unsafe void update()
        {
            
            fixed (float* ptr = &Zoom)
            {
                Vec2 pos = currentState.PerformOperation(this, ptr);

                if (pos != null) { FocusPosition = pos; }
            }
            
        }

        
    }
}
