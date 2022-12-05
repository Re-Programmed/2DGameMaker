using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Collisions;
using _2DGameMaker.Objects.Stationaries;
using _2DGameMaker.Rendering.Cameras;
using _2DGameMaker.Rendering.Display;
using _2DGameMaker.Rendering.Sprites;
using _2DGameMaker.Utils.AssetManagment;
using _2DGameMaker.Utils.Math;
using GLFW;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Resources;
using static _2DGameMaker.OpenGL.GL;

namespace _2DGameMaker.Game
{
    public abstract class Game
    {
        const string VertexShader = "./Shaders/sprite.vs";
        const string FragShader = "./Shaders/sprite.frag";

        readonly string GameTitle;

        private Vec4 clearColor = Vec4.Zero;
        public void SetClearColor(Vec4 color) { clearColor = color; }

        protected ObjectLayer[] objects = new ObjectLayer[] { new ObjectLayer(), new ObjectLayer(), new ObjectLayer() };

        protected Camera2d cam;

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

        private Dictionary<GameObject, int> instantiations = new Dictionary<GameObject, int>();
        public void Instantiate(GameObject obj, int layer)
        {
            instantiations.Add(obj, layer);
        }

        protected virtual void LoadContent()
        {
            cam = new Camera2d(Vec2.Zero, 1f);

            AssetManager.LoadSpriteShader(VertexShader, FragShader, null, "sprite");

            AssetManager.GetSpriteShader("sprite").Use().SetInt("image", 0, false);
            AssetManager.GetSpriteShader("sprite").SetMatrix4x4("projection", cam.GetProjectionMatrix(), false);

            SpriteRenderer.InitShader(AssetManager.GetSpriteShader("sprite"));
            SpriteRenderer.InitRenderData();

            AssetManager.GetPacks();
        }

        protected abstract void Init();

        protected virtual void Update()
        {
            if(instantiations.Count > 0)
            {
                foreach (KeyValuePair<GameObject, int> valuePair in instantiations)
                {
                    objects[valuePair.Value].objects.Add(valuePair.Key);
                }

                instantiations.Clear();
            }
        }

        protected void RenderScreen()
        {
            ClearColor();
            glClear(GL_COLOR_BUFFER_BIT);

            glBindVertexArray(SpriteRenderer.quadVAO);

            glDisable(GL_DEPTH_TEST);
            glEnable(GL_BLEND);

            glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

            Render();

            foreach (ObjectLayer objlay in objects)
            {
                if(objlay.objects.Count > 0)
                {
                    foreach (GameObject obj in objlay.objects)
                    {
                        if (CollisionCheck.BoxCheck(obj.GetPosition(), cam.FocusPosition, new Vec2(DisplayManager.WindowSize.X, DisplayManager.WindowSize.Y)))
                        {
                            if (!obj.GetLoaded())
                            {
                                obj.SetLoaded(true);
                            }
                            SpriteRenderer.DrawSprite(cam, obj, obj.texture.GetTexture(), obj.texture.GetColor());
                        }
                        else if (obj.GetLoaded() && !obj.GetAlwaysLoad())
                        {
                            obj.SetLoaded(false);
                        }
                    }
                }
            }
            

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