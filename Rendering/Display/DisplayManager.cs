using GLFW;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Text;
using StbImageSharp;
using static _2DGameMaker.OpenGL.GL;

namespace _2DGameMaker.Rendering.Display
{
    static class DisplayManager
    {
        public static Window Window { get; set; }
        public static Vector2 WindowSize { get; set; }

        public static void SetBordered(bool bordered)
        {
            Glfw.SetWindowAttribute(Window, WindowAttribute.Decorated, bordered);
        }

        public static void SetResizable(bool resizeable)
        {
            Glfw.SetWindowAttribute(Window, WindowAttribute.Resizable, resizeable);
        }

        public static void SetAspectRatio(byte n, byte d)
        {
            Glfw.SetWindowAspectRatio(Window, n, d);
        }

        public static void Maximize()
        {
            Glfw.MaximizeWindow(Window);
        }

        public static void SetTitle(string title)
        {
            Glfw.SetWindowTitle(Window, title);
        }

        public static unsafe void SetIcon(string[] file)
        {
            GLFW.Image[] images = new GLFW.Image[file.Length];
            int i = 0;
            foreach (string s in file)
            {
                using (var stream = File.OpenRead(s))
                {
                    ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

                    int width = image.Width;
                    int height = image.Height;

                    fixed (byte* bytePtr = image.Data)
                    {
                        int* intPtr = (int*)bytePtr;

                        images[i] = new GLFW.Image(width, height, (IntPtr)intPtr);
                    }

                }
                i++;

            }

            Glfw.SetWindowIcon(Window, images.Length, images);
        }

        public static void Fullscreen(Monitor monitor)
        {
            WindowSize = new Vector2(monitor.WorkArea.Width, monitor.WorkArea.Height);
            Glfw.SetWindowMonitor(Window, monitor, 0, 0, (int)WindowSize.X, (int)WindowSize.Y, Glfw.GetVideoMode(monitor).RefreshRate);
        }

        public static void UnFullscreen(int width, int height, Monitor monitor)
        {
            Rectangle screen = Glfw.PrimaryMonitor.WorkArea;
            Glfw.SetWindowMonitor(Window, Monitor.None, (screen.Width - width) / 2, (screen.Height - height) / 2, width, height, Glfw.GetVideoMode(monitor).RefreshRate);

            WindowSize = new Vector2(width, height);
        }

        public static void CreateWindow(int width, int height, string title, Monitor? fullscreen = null)
        {
            WindowSize = new Vector2(width, height);

            Glfw.Init();

            //Use OpenGL 3.3
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);

            //Make the window focused and not resizeable
            Glfw.WindowHint(Hint.Focused, true);
            
            Glfw.WindowHint(Hint.Resizable, false);
                
            //Create Window
            Window = Glfw.CreateWindow(width, height, title, Monitor.None, Window.None);

            if (Window == Window.None)
            {
                //Error
                return;
            }

            Rectangle screen = Glfw.PrimaryMonitor.WorkArea;

            int x = (screen.Width - width) / 2;
            int y = (screen.Height - height) / 2;

            Glfw.SetWindowPosition(Window, x, y);

            Glfw.MakeContextCurrent(Window);
            Import(Glfw.GetProcAddress);

            glViewport(0, 0, width, height);
            Glfw.SwapInterval(1); //Vsync off, 1 is on

            if (fullscreen != null) { Fullscreen((Monitor)fullscreen); }
        }

        public static void CloseWindow()
        {
            Glfw.Terminate();
        }
    }
}
