﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using StbImageSharp;
using _2DGameMaker.Rendering.Sprites;
using static _2DGameMaker.OpenGL.GL;

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
                string p_file_name = Path.GetFileNameWithoutExtension(f);
                CreateTextureDictionary(p_file_name);
                Dictionary<byte, object> keyValues = new Dictionary<byte, object>();

                string[] content = File.ReadAllText(f).Split("\n");

                foreach(string fd in content)
                {
                    string[] file_data = fd.Split(" ");

                    Console.WriteLine("FILE LOCATED: " + getResourceInformationH(file_data, ResourceInformation.H_FILE_TYPE) + ", " + getResourceInformationH(file_data, ResourceInformation.H_FILE_ID));

                    switch ((ResourceType)getResourceInformationH(file_data, ResourceInformation.H_FILE_TYPE))
                    {
                        case ResourceType.PNG_IMAGE:
                            keyValues.Add(getResourceInformationH(file_data, ResourceInformation.H_FILE_ID), DecodePng(getResourceInformation(file_data, ResourceInformation.S_FILE_DATA), true));
                            break;
                        case ResourceType.DATA:
                            Dictionary<string, object> ret = DataDecode(getResourceInformation(file_data, ResourceInformation.S_FILE_DATA), keyValues);
                            int i = ret.Count - 1;
                            foreach(KeyValuePair<string, object> ret_v in ret)
                            {
                                Console.WriteLine(ret_v.Key);

                                AddFileToSystem(ret_v.Value, i == 0 ? ret_v.Key : ret_v.Key.Remove(ret_v.Key.Length - 1), p_file_name);
                                i--;
                            }
                            break; 
                        default:
                            break;
                    }
                }
            }
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





        public static Dictionary<string, SpriteShader> SpriteShaders = new Dictionary<string, SpriteShader>();
        public static Dictionary<string, Dictionary<string, Texture2D>> Textures = new Dictionary<string, Dictionary<string, Texture2D>>();

        public static SpriteShader LoadSpriteShader(string vShaderFile, string fShaderFile, string gShaderFile, string name)
        {
            SpriteShaders[name] = loadShaderFromFile(vShaderFile, fShaderFile, gShaderFile);
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

        private static SpriteShader loadShaderFromFile(string vShaderFile, string fShaderFile, string gShaderFile = null)
        {
            string vertexCode = "";
            string fragmentCode = "";
            string geometryCode = "";

            using (StreamReader reader = new StreamReader(vShaderFile))
            {
                vertexCode = reader.ReadToEnd();
            }

            using (StreamReader reader = new StreamReader(fShaderFile))
            {
                fragmentCode = reader.ReadToEnd();
            }

            if (gShaderFile != null)
            {
                using (StreamReader reader = new StreamReader(gShaderFile))
                {
                    geometryCode = reader.ReadToEnd();
                }
            }

            string vShaderCode = vertexCode;
            string fShaderCode = fragmentCode;
            string gShaderCode = geometryCode;

            SpriteShader shader = new SpriteShader();
            shader.Compile(vShaderCode, fShaderCode, gShaderCode != null ? gShaderCode : null);
            return shader;
        }
    }
}
