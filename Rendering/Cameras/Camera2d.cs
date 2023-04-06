using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using GLFW;
using _2DGameMaker.Rendering.Display;
using _2DGameMaker.Game;
using _2DGameMaker.Utils.Math;
using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Stationaries;

#pragma warning disable

namespace _2DGameMaker.Rendering.Cameras
{
    public class Camera2d
    {
        public Vec2 FocusPosition { get; set; }
        public float Zoom;

        public bool DisableZoom;

        public Camera2d(Vec2 focusPosition, float zoom)
        {
            this.FocusPosition = focusPosition;
            this.Zoom = zoom;
        }

        public Matrix4x4 GetProjectionMatrix()
        {
            float left = FocusPosition.X - DisplayManager.WindowSize.X / 2f;
            float right = FocusPosition.X + DisplayManager.WindowSize.X / 2f;
            float top = FocusPosition.Y - DisplayManager.WindowSize.Y / 2f;
            float bottom = FocusPosition.Y + DisplayManager.WindowSize.Y / 2f;

            Matrix4x4 orthoMatrix = Matrix4x4.CreateOrthographicOffCenter(left, right, bottom, top, 0.01f, 100f);
            Matrix4x4 zoomMatrix = Matrix4x4.CreateScale(Zoom);

            return orthoMatrix * zoomMatrix;
        }

        public Vec2 MouseToWorldCoords(Vector2 mouse)
        {
            return new Vec2(mouse.X / Zoom + FocusPosition.X - (DisplayManager.WindowSize.X/Zoom) / 2f, mouse.Y / Zoom + (FocusPosition.Y - (DisplayManager.WindowSize.Y/Zoom) / 2f));
        }


        /// <summary>
        /// Lerps the camera twards a position at a speed.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="speed"></param>
        public void LerpTwards(Vec2 target, float speed)
        {
            FocusPosition.Lerp(target, speed);
            
        }

        Vec2 LerpGrid;

        /// <summary>
        /// Makes the camera move in a grid when a position leaves its area.
        /// </summary>
        public void GridCheck(Vec2 checkPosition)
        {
            if(LerpGrid != null)
            {
                LerpTwards(LerpGrid, GameTime.DeltaTime * 3f);
            }

            if(checkPosition.X > FocusPosition.X - 100f + (DisplayManager.WindowSize.X * 1 / Zoom) / 2f)
            {
                LerpGrid = new Vec2(FocusPosition.X + (DisplayManager.WindowSize.X * 1 / Zoom) / 2f, FocusPosition.Y);
            }

            if (checkPosition.X < FocusPosition.X - (DisplayManager.WindowSize.X * 1 / Zoom) / 2f)
            {
                LerpGrid = new Vec2(FocusPosition.X - (DisplayManager.WindowSize.X * 1 / Zoom) / 2f, FocusPosition.Y);
            }

            if (checkPosition.Y > FocusPosition.Y - 100f + (DisplayManager.WindowSize.Y * 1 / Zoom) / 2f)
            {
                LerpGrid = new Vec2(FocusPosition.X, FocusPosition.Y + (DisplayManager.WindowSize.Y * 1 / Zoom) / 2f);
            }

            if (checkPosition.Y < FocusPosition.Y - (DisplayManager.WindowSize.Y * 1 / Zoom) / 2f)
            {
                LerpGrid = new Vec2(FocusPosition.X, FocusPosition.Y - (DisplayManager.WindowSize.Y * 1 / Zoom) / 2f);
            }
        }
    }
}
