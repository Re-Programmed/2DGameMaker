using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace _2DGameMaker.Utils.AssetManagment
{
    public static class AppDataManager
    {
        public const string B64OverrideSuffix = "b6d_";

        static string directory;
        static Dictionary<string, string> dataFiles = new Dictionary<string, string>();

        public static void RegisterAppDataFolder(string applicationName, string companyName = "")
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + (companyName == "" ? "\\" : "\\" + companyName) + "\\" + applicationName;
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
        public static void UpdateFile(string fileName, object data, bool overrideB64 = false)
        {
            string fname =  fileName + (overrideB64 ? B64OverrideSuffix : "");
            if (dataFiles.ContainsKey(fname))
            {
                dataFiles[fname] = XMLManager.ToXML(data);
                return;
            }

            registerFile(fname, data);
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

        /// <summary>
        /// Saves all the files to the hard drive.
        /// </summary>
        public static void SaveFiles()
        {
            foreach (KeyValuePair<string, string> fileData in dataFiles)
            {
                string path = directory + "\\" + fileData.Key + ".00.spak";

                int i = 0;
                string[] split = fileData.Key.Replace(B64OverrideSuffix, "").Split("\\");
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

                if (fileData.Key.EndsWith(B64OverrideSuffix))
                {
                    File.WriteAllText(path, fileData.Value);
                }
                else
                {
                    File.WriteAllText(path, Convert.ToBase64String(Encoding.UTF8.GetBytes(fileData.Value)));
                }
            }
        }
        
        /// <summary>
        /// Loads all files from hard drive.
        /// </summary>
        public static void LoadFiles()
        {
            foreach(string file in Directory.GetFiles(directory, "*", SearchOption.AllDirectories))
            {
                string fname = file.Replace(directory + "\\", "").Replace(".00.spak", "");
                string s = fname.EndsWith(B64OverrideSuffix) ? File.ReadAllText(file) : Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllText(file)));
                dataFiles.Add(fname, s);
            }
        }
    }
}
