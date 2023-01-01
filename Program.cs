using System;
using GLFW;
using _2DGameMaker.Game;

namespace _2DGameMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            TemplateGame.TemplateGame tg = new TemplateGame.TemplateGame("Template Game");
            tg.Run(1920, 1080, Glfw.PrimaryMonitor);
        }
    }
}
