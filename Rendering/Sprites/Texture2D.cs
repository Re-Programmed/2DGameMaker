using System;
using System.Collections.Generic;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using static _2DGameMaker.OpenGL.GL;

namespace _2DGameMaker.Rendering.Sprites
{
    class Texture2D
    {
        public uint ID;

        public int Width, Height;

        public int Internal_Format;
        public int Image_Format;

        public int Wrap_S;
        public int Wrap_T;

        public int Filter_Min;
        public int Filter_Max;

        public unsafe Texture2D()
        {
            Width = 0;
            Height = 0;

            Internal_Format = GL_RGB;
            Image_Format = GL_RGB;
            Wrap_S = GL_REPEAT;
            Wrap_T = GL_REPEAT;
            Filter_Min = GL_NEAREST;
            Filter_Max = GL_NEAREST;
            ID = glGenTexture();
        }

        public unsafe void Generate(int Width, int Height, byte[] data)
        {
            this.Width = Width;
            this.Height = Height;

            glBindTexture(GL_TEXTURE_2D, ID);

            GCHandle pinnedArray = GCHandle.Alloc(data, GCHandleType.Pinned);
            IntPtr pointer = pinnedArray.AddrOfPinnedObject();
            glTexImage2D(GL_TEXTURE_2D, 0, Internal_Format, Width, Height, 0, Image_Format, GL_UNSIGNED_BYTE, pointer);
            pinnedArray.Free();

    
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, Wrap_S);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, Wrap_T);

            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, Filter_Min);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, Filter_Max);

            glBindTexture(GL_TEXTURE_2D, 0);
        }

        public void Bind()
        {
            glBindTexture(GL_TEXTURE_2D, ID);
        }
    }
}
