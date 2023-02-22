using System;
using GLFW;
using System.Windows.Media;
using System.IO;
using System.Media;
using _2DGameMaker.Utils.AssetManagment;

namespace _2DGameMaker
{
    public struct CommonSaveData
    {
        public int LaunchCount;

        public CommonSaveData(int LaunchCount)
        {
            this.LaunchCount = LaunchCount;
        }
    }

    class Program
    {
        public static CommonSaveData CommonSaveData;
        public const string COMPANY_NAME = "Re_Programmed";
        public const string APPLICATION_NAME = "Template Game";

        public delegate void ProgramClose();
        public static ProgramClose Close;

        static void Main(string[] args)
        {
            //Run on start
            AppDataManager.RegisterAppDataFolder(APPLICATION_NAME, COMPANY_NAME);
            AppDataManager.LoadFiles();
            //End run on start

            AppDataManager.GetFile("application\\common", out CommonSaveData);
            Console.WriteLine(CommonSaveData.LaunchCount);

            //TemplateGame.TemplateGame tg = new TemplateGame.TemplateGame(APPLICATION_NAME);
            //tg.Run(1920, 1080, Glfw.PrimaryMonitor);

            StageCreator.StageCreator sc = new StageCreator.StageCreator(APPLICATION_NAME);
            sc.Run(1920, 1080, Glfw.PrimaryMonitor);


            AppDataManager.UpdateFile("application\\common", new CommonSaveData(CommonSaveData.LaunchCount + 1));

            Close?.Invoke();

            //Run on close
            AppDataManager.SaveFiles();
        }
    }
}
