using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using StbImageSharp;

namespace _2DGameMaker.Utils.AssetManagment
{
    class AssetManager
    {
        const string PackDIR = "./Assets";
        enum ResourceType:byte
        {
            DATA = 1,
            PNG_IMAGE = 56
        }

        //H prefix = hex number
        //S prefix = string
        //D suffix = data file attribute
        enum ResourceInformation:uint
        {
            H_FILE_TYPE = 0,
            H_FILE_ID = 1,
            H_FILE_ID_D = 0,
            S_FILE_NAME_D = 1,
            S_FILE_DATA = 2
        }
        //Pass in the line of a decoded PAK file and a resource type to return.
        static string getResourceInformation(string[] resource, ResourceInformation fetch)
        {
            return resource[(uint)fetch];
        }

        //Same as getResourceInformation but returns a byte.
        static byte getResourceInformationB(string[] resource, ResourceInformation fetch)
        {
            return byte.Parse(getResourceInformation(resource, fetch));
        }

        //Same as getResourceInformation but returns a byte from hex.
        static byte getResourceInformationH(string[] resource, ResourceInformation fetch)
        {
            return Convert.ToByte(getResourceInformation(resource, fetch), 16);
        }

        public static void GetPacks()
        {
            foreach (string f in Directory.GetFiles(PackDIR))
            {
                Dictionary<byte, object> keyValues = new Dictionary<byte, object>();

                string[] content = File.ReadAllText(f).Split("\n");

                foreach(string fd in content)
                {
                    string[] file_data = fd.Split(" ");

                    Console.WriteLine("FILE LOCATED: " + getResourceInformationH(file_data, ResourceInformation.H_FILE_TYPE));

                    switch ((ResourceType)getResourceInformationH(file_data, ResourceInformation.H_FILE_TYPE))
                    {
                        case ResourceType.PNG_IMAGE:
                            keyValues.Add(getResourceInformationH(file_data, ResourceInformation.H_FILE_ID), DecodePng(getResourceInformation(file_data, ResourceInformation.S_FILE_DATA)));
                            break;
                        case ResourceType.DATA:
                            var ret = DataDecode(getResourceInformation(file_data, ResourceInformation.S_FILE_DATA), keyValues);
                            foreach(KeyValuePair<string, object> ret_v in ret)
                            {
                                Console.WriteLine($"{ret_v.Key}, {ret_v.Value}");
                            }
                            break; 
                        default:
                            break;
                    }
                }
            }
        }

        public static Dictionary<string, object> DataDecode(string b64, Dictionary<byte, object> reference)
        {
            Dictionary<string, object> return_d = new Dictionary<string, object>();

            byte[] b64_d = Convert.FromBase64String(b64);
            string data = ASCIIEncoding.ASCII.GetString(b64_d);

            foreach(string s in data.Split("\n"))
            {
                //CHECK FILE TYPE
                if(s.StartsWith("!"))
                {
                    if(!s.Contains("DATA"))
                    {
                        Console.WriteLine("File type not specified for '.DATA' file.");
                        break;
                    }

                    continue;
                }

                string[] ref_data = s.Split(" ");

                return_d.Add(getResourceInformation(ref_data, ResourceInformation.S_FILE_NAME_D), reference[getResourceInformationH(ref_data, ResourceInformation.H_FILE_ID_D)]);
            }

            return return_d;
        }

        public static ImageResult DecodePng(string b64)
        {
            ImageResult image;

            byte[] data = Convert.FromBase64String(b64);
            using (var stream = new MemoryStream(data, 0, data.Length))
            {
                image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
            }
            
            return image;
        }
    }
}
