﻿using _2DGameMaker.Game;
using System;
using GLFW;
using _2DGameMaker.Utils.AssetManagment;

namespace _2DGameMaker
{
    class Program
    {
        static void Main(string[] args)
        {            
            TemplateGame tg = new TemplateGame("Template Game");
            tg.Run(1920, 1080, Glfw.PrimaryMonitor);
        }
    }
}
