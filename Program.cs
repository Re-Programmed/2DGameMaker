using _2DGameMaker.Game;
using System;
using GLFW;

namespace _2DGameMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            TemplateGame tg = new TemplateGame("Template Game");
            tg.Run(1920, 1080, Glfw.PrimaryMonitor);
            
        }
    }
}
