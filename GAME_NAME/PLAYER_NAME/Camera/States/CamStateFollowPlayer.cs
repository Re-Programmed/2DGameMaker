using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.GAME_NAME.PLAYER_NAME.Camera.States
{
    public class CamStateFollowPlayer : CameraState
    {
        readonly float camFollowSpeed;
        readonly Vec2 camFollowOffset;

        public CamStateFollowPlayer(float camFollowSpeed)
        {
            this.camFollowSpeed = camFollowSpeed;
            camFollowOffset = Vec2.Zero;
        }

        public CamStateFollowPlayer(float camFollowSpeed, Vec2 camFollowOffset)
        {
            this.camFollowSpeed = camFollowSpeed;
            this.camFollowOffset = camFollowOffset;
        }

        public unsafe override Vec2 PerformOperation(DynamicCamera camera, float* zoom)
        {
            camera.FocusPosition = _2DGameMaker.Utils.Math.Math.Lerp(camera.FocusPosition, GameName.ThePlayer.GetGameObject().GetPosition(), camFollowSpeed);
            return null;
        }
    }
}
