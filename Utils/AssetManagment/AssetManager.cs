using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using StbImageSharp;
using _2DGameMaker.Rendering.Sprites;
using static _2DGameMaker.OpenGL.GL;
using _2DGameMaker.Game.Stages;

namespace _2DGameMaker.Utils.AssetManagment
{
    class AssetManager
    {
        const string PackDIR = "./Assets";
        enum ResourceType:byte
        {
            DATA = 1,
            PNG_IMAGE = 56,
            FRAG_SHADER = 2,
            VERT_SHADER = 3,
            AUDIO = 4,
            STAGE = 5
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
            string vShader = "", fShader = "";
            foreach (string f in Directory.GetFiles(PackDIR))
            {
                string p_file_name = Path.GetFileNameWithoutExtension(f);
                CreateTextureDictionary(p_file_name);
                Dictionary<byte, object> keyValues = new Dictionary<byte, object>();

                string[] content = File.ReadAllText(f).Split("\n");

                foreach(string fd in content)
                {
                    string[] file_data = fd.Split(" ");

                    switch ((ResourceType)getResourceInformationH(file_data, ResourceInformation.H_FILE_TYPE))
                    {
                        case ResourceType.PNG_IMAGE:
                            keyValues.Add(getResourceInformationH(file_data, ResourceInformation.H_FILE_ID), DecodePng(getResourceInformation(file_data, ResourceInformation.S_FILE_DATA), true));
                            break;
                            
                        case ResourceType.AUDIO:
                            keyValues.Add(getResourceInformationH(file_data, ResourceInformation.H_FILE_ID), DecodeAudio(getResourceInformation(file_data, ResourceInformation.S_FILE_DATA)));
                            break;

                        case ResourceType.STAGE:
                            keyValues.Add(getResourceInformationH(file_data, ResourceInformation.H_FILE_ID), "STAGE!" + DecodeB64String(getResourceInformation(file_data, ResourceInformation.S_FILE_DATA)));
                            break;
                        case ResourceType.VERT_SHADER:
                            vShader = "#" + DecodeB64String(getResourceInformation(file_data, ResourceInformation.S_FILE_NAME_D));
                            break;
                        case ResourceType.FRAG_SHADER:
                            fShader = "#" + DecodeB64String(getResourceInformation(file_data, ResourceInformation.S_FILE_NAME_D));
                            break;

                        case ResourceType.DATA:
                            Dictionary<string, object> ret = DataDecode(getResourceInformation(file_data, ResourceInformation.S_FILE_DATA), keyValues);
                            int i = ret.Count - 1;
                            foreach (KeyValuePair<string, object> ret_v in ret)
                            {
                                AddFileToSystem(ret_v.Value, i == 0 ? ret_v.Key : ret_v.Key.Remove(ret_v.Key.Length - 1), p_file_name, new string[] { f });
                                i--;
                            }
                            break; 
                        default:
                            break;
                    }
                }

            }

            LoadSpriteShader(vShader, fShader, "sprite");
        }

        /// <summary>
        /// Adds a file based on its type.
        /// </summary>
        private static void AddFileToSystem(object file, string name, string pack_file_name, string[] attributes = null)
        {
            switch (file)
            {
                case ImageResult image:
                    LoadTexture(image, true, name, pack_file_name);
                    break;
                case byte[] audio:
                    //SoundManager.InitSound(name, audio, attributes[0]);
                    break;
                case string str:
                    if (str.StartsWith("STAGE!"))
                    {
                        StageManager.LoadStage(str.Remove(0, 6), name);
                        break;
                    }
                    break;
                default:
                   break;
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

        public static string DecodeB64String(string b64)
        {
            return Encoding.Default.GetString(Convert.FromBase64String(b64));
        }

        public static ImageResult DecodePng(string b64, bool alpha)
        {
            ImageResult image;

            byte[] data = Convert.FromBase64String(b64);
            using (var stream = new MemoryStream(data, 0, data.Length))
            {
                image = ImageResult.FromStream(stream, alpha ? ColorComponents.RedGreenBlueAlpha : ColorComponents.RedGreenBlue);
                stream.Close();
            }
            
            return image;
        }

        
        public static byte[] DecodeAudio(string b64)
        {
            return Convert.FromBase64String(b64);
        }
        


        public static Dictionary<string, SpriteShader> SpriteShaders = new Dictionary<string, SpriteShader>();
        public static Dictionary<string, Dictionary<string, Texture2D>> Textures = new Dictionary<string, Dictionary<string, Texture2D>>();

        public static SpriteShader LoadSpriteShader(string vShaderFile, string fShaderFile, string name)
        {
            SpriteShaders[name] = loadShaderFromFile(vShaderFile, fShaderFile);
            return SpriteShaders[name];
        }

        public static SpriteShader GetSpriteShader(string name)
        {
            return SpriteShaders[name];
        }

        public unsafe static void ClearShaders()
        {
            foreach (SpriteShader shader in SpriteShaders.Values)
            {
                glDeleteProgram(shader.ID);
            }

            foreach (Dictionary<string, Texture2D> texture_pack in Textures.Values)
            {
                foreach(Texture2D texture in texture_pack.Values)
                {
                    glDeleteTexture(texture.ID);
                }
            }
        }
        public static Texture2D LoadTexture(string file, bool alpha, string name, string dictionary)
        {
            Textures[dictionary].Add(name, loadTextureFromFile(file, alpha));
            return Textures[dictionary][name];
        }

        public static Texture2D LoadTexture(ImageResult image, bool alpha, string name, string dictionary)
        {
            Textures[dictionary].Add(name, loadTextureFromImageResult(image, alpha));
            return Textures[dictionary][name];
        }

        public static Texture2D GetTexture(string name, string dictionary)
        {
             return Textures[dictionary][name];
        }

        public static bool TextureExists(string name, string dictionary)
        {
            return Textures[dictionary].ContainsKey(name);
        }

        public static void CreateTextureDictionary(string name)
        {
            Textures.Add(name, new Dictionary<string, Texture2D>());
        }

        private static unsafe Texture2D loadTextureFromFile(string file, bool alpha)
        {
            using (var stream = File.OpenRead(file))
            {
                ImageResult image = null;
                image = ImageResult.FromStream(stream, alpha ? ColorComponents.RedGreenBlueAlpha : ColorComponents.RedGreenBlue);

                return loadTextureFromImageResult(image, alpha);
            }
        }

        private static unsafe Texture2D loadTextureFromImageResult(ImageResult image, bool alpha = true)
        {
            Texture2D texture = new Texture2D();

            if (alpha)
            {
                texture.Internal_Format = GL_RGBA;
                texture.Image_Format = GL_RGBA;
            }

            texture.Generate(image.Width, image.Height, image.Data);

            return texture;
        }

        private static SpriteShader loadShaderFromFile(string vShaderFile, string fShaderFile)
        {
            SpriteShader shader = new SpriteShader();
            shader.Compile(vShaderFile, fShaderFile);
            return shader;
        }
    }
}
