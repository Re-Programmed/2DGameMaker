using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace _2DGameMaker.Utils.AssetManagment
{
    public static class AppDataManager
    {
        static string directory;
        static Dictionary<string, string> dataFiles = new Dictionary<string, string>();

        public static void RegisterAppDataFolder(string applicationName, string companyName = "")
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + (companyName == "" ? "\\" : "\\" + companyName) + "\\" + applicationName;
            directory = dir;

            if (Directory.Exists(dir)) { return; }

            Directory.CreateDirectory(dir);
        }

        private static void registerFile(string fileName, object data)
        {
            dataFiles.Add(fileName, XMLManager.ToXML(data));
        }

        /// <summary>
        /// Updates the data of a file to be saved. Will create a new file if the specified one does not exist.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="data">The object to write to the file.</param>
        public static void UpdateFile(string fileName, object data)
        {
            if(dataFiles.ContainsKey(fileName))
            {
                dataFiles[fileName] = XMLManager.ToXML(data);
                return;
            }

            registerFile(fileName, data);
        }

        /// <summary>
        /// Fetches the data from a loaded file.
        /// </summary>
        /// <param name="fileName">The file to fetch.</param>
        /// <param name="data">Returns as the data from the file. If no file exists data will be set to the default.</param>
        /// <returns>If the file specified exists.</returns>
        public static bool GetFile<T>(string fileName, out T data)
        {
            if(dataFiles.ContainsKey(fileName))
            {
                data = XMLManager.LoadFromXMLString<T>(dataFiles[fileName]);
                return true;
            }

            data = default(T);
            return false;
        }

        public static void SaveFiles()
        {
            foreach (KeyValuePair<string, string> fileData in dataFiles)
            {
                string path = directory + "\\" + fileData.Key + ".00.spak";

                int i = 0;
                string[] split = fileData.Key.Split("\\");
                foreach (string check in split)
                {
                    if (i == split.Length - 1) { break; }
                    if(!Directory.Exists(directory + "\\" + check))
                    {
                        Directory.CreateDirectory(directory + "\\" + check);
                    }

                    i++;
                }

                if (!File.Exists(path)) { File.Create(path).Close(); }

                File.WriteAllText(path, Convert.ToBase64String(Encoding.UTF8.GetBytes(fileData.Value)));
            }
        }

        public static void LoadFiles()
        {
            foreach(string file in Directory.GetFiles(directory, "*", SearchOption.AllDirectories))
            {
                string s = Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllText(file)));
                dataFiles.Add(file.Replace(directory + "\\", "").Replace(".00.spak", ""), s);
            }
        }
    }
}
