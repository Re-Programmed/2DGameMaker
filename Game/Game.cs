using _2DGameMaker.Rendering.Display;
using _2DGameMaker.Utils.Math;
using GLFW;
using System;
using static _2DGameMaker.OpenGL.GL;

namespace _2DGameMaker.Game
{
    public abstract class Game
    {
        readonly string GameTitle;

        private Vec4 clearColor = Vec4.Zero;
        public void SetClearColor(Vec4 color) { clearColor = color; }

        public Game(string title)
        {
            GameTitle = title;
        }

        public void Run(int width, int height, Monitor? fullscreen = null)
        {
            DisplayManager.CreateWindow(width, height, GameTitle, fullscreen);

            LoadContent();
            Init();

            while (!Glfw.WindowShouldClose(DisplayManager.Window))
            {
                GameTime.DeltaTime = (float)Glfw.Time - GameTime.TotalElapsedSeconds;
                GameTime.TotalElapsedSeconds = (float)Glfw.Time;

                Update();

                Glfw.PollEvents();

                RenderScreen();

                LateUpdate();
            }

            Close();

            DisplayManager.CloseWindow();
        }

        protected abstract void LoadContent();
        protected abstract void Init();

        protected abstract void Update();
        protected void RenderScreen()
        {
            ClearColor();
            glClear(GL_COLOR_BUFFER_BIT);

            //glBindVertexArray(sr.quadVAO);

            glDisable(GL_DEPTH_TEST);
            glEnable(GL_BLEND);

            glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

            Render();

            glBindVertexArray(0);

            Glfw.SwapBuffers(DisplayManager.Window);
        }

        protected abstract void Render();

        protected abstract void LateUpdate();

        protected abstract void Close();



        private void ClearColor()
        {
            glClearColor(clearColor.X, clearColor.Y, clearColor.Z, clearColor.W);
        }
    }
}