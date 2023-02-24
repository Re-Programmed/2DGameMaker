using System;
using System.Collections.Generic;
using System.Text;
using _2DGameMaker.Utils.AssetManagment;
using _2DGameMaker.Objects;

namespace _2DGameMaker.Game.Stages
{
    public static class StageManager
    {
        private static Dictionary<string, Stage> stages = new Dictionary<string, Stage>();

        /// <summary>
        /// Loads XML data into the stage manager.
        /// </summary>
        /// <param name="XMLText">The XML text.</param>
        public static void LoadStage(string XMLText, string name)
        {
            stages.Add(name, XMLManager.LoadFromXMLString<Stage>(XMLText));
        }

        public static Stage? RegisterStage(string name, Stage stage)
        {
            if (!stages.ContainsKey(name))
            {
                stages.Add(name, stage);
                return stage;
            }

            return null;
        }

        public static void GenerateStage(string name)
        {
            foreach (StageObject obj in stages[name].objects)
            {
                Game.INSTANCE.Instantiate(obj.GetObject(), obj.Layer);
            }
        }

        public static void GenerateStage(string name, out GameObject[] gameObjects)
        {
            List<GameObject> go = new List<GameObject>();
            foreach (StageObject obj in stages[name].objects)
            {
                GameObject o = obj.GetObject();
                go.Add(o);
                Game.INSTANCE.Instantiate(o, obj.Layer);
            }

            gameObjects = go.ToArray();
        }

        public static StageObject[] GetStageObjects(string name)
        {
            return stages[name].objects;
        }

        public static void SaveStageInstance(StageObject[] objects)
        {
            Stage s = new Stage(objects);

            Console.WriteLine(XMLManager.ToXML(s));
        }

        /// <summary>
        /// Converts an array of GameObjects to an array of StageObjects. THIS IS PURELY FOR DEVELOPER USE AND CAN BE DELETED BEFORE RELEASE OF GAME.
        /// </summary>
        /// <param name="objects">GameObject array to convert.</param>
        /// <returns>StageObject array.</returns>
        public static StageObject[] ConvertObjectArray(GameObject[] objects)
        {
            List<StageObject> stageObjects = new List<StageObject>();

            foreach (GameObject obj in objects)
            {
                stageObjects.Add(new StageObject(obj));
            }

            return stageObjects.ToArray();
        }
    }
}
