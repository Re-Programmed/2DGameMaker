using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using static _2DGameMaker.OpenGL.GL;

namespace _2DGameMaker.Rendering.Sprites
{
    class SpriteShader
    {
        public uint ID;

        public SpriteShader()
        {

        }

        public SpriteShader Use()
        {
            glUseProgram(ID);
            return this;
        }

        public void Compile(string vertexSource, string fragmentSource, string geometrySource = null)
        {
            uint sVertex = 0, sFragment = 0, gShader = 0;

            sVertex = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(sVertex, vertexSource);
            glCompileShader(sVertex);
            checkCompileErrors(sVertex, "VERTEX");

            sFragment = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(sFragment, fragmentSource);
            glCompileShader(sFragment);
            checkCompileErrors(sFragment, "FRAGMENT");

            if(geometrySource != null && geometrySource != "")
            {
                gShader = glCreateShader(GL_GEOMETRY_SHADER);
                glShaderSource(gShader, geometrySource);
                glCompileShader(gShader);
                checkCompileErrors(gShader, "GEOMETRY");
            }

            ID = glCreateProgram();
            glAttachShader(ID, sVertex);
            glAttachShader(ID, sFragment);
            if(geometrySource != null)
            {
                glAttachShader(ID, gShader);
            }

            glLinkProgram(ID);
            checkCompileErrors(ID, "PROGRAM");

            glDeleteShader(sVertex);
            glDeleteShader(sFragment);
            if(geometrySource != null)
            {
                glDeleteShader(gShader);
            }
        }

        public void SetFloat(string name, float value, bool useShader)
        {
            if(useShader)
            {
                Use();
            }

            int location = glGetUniformLocation(ID, name);
            glUniform1f(location, value);
        }

        public void SetInt(string name, int value, bool useShader)
        {
            if (useShader)
            {
                Use();
            }

            int location = glGetUniformLocation(ID, name);
            glUniform1i(location, value);
        }

        public void SetVector2f(string name, float x, float y, bool useShader)
        {
            if (useShader)
            {
                Use();
            }

            int location = glGetUniformLocation(ID, name);
            glUniform2f(location, x, y);
        }

        public void SetVector2f(string name, Vector2 value, bool useShader)
        {
            if(useShader)
            {
                Use();
            }

            int location = glGetUniformLocation(ID, name);
            glUniform2f(location, value.X, value.Y);
        }

        public void SetVector3f(string name, Vector3 value, bool useShader)
        {
            if (useShader)
            {
                Use();
            }

            int location = glGetUniformLocation(ID, name);
            glUniform3f(location, value.X, value.Y, value.Z);
        }

        public void SetVector3f(string name, float x, float y, float z, bool useShader)
        {
            if (useShader)
            {
                Use();
            }

            int location = glGetUniformLocation(ID, name);
            glUniform3f(location, x, y, z);
        }

        public void SetVector4f(string name, Vector4 value, bool useShader)
        {
            if(useShader)
            {
                Use();
            }

            int location = glGetUniformLocation(ID, name);
            glUniform4f(location, value.X, value.Y, value.Z, value.W);
        }

        public void SetVector4f(string name, float x, float y, float z, float w, bool useShader)
        {
            if(useShader)
            {
                Use();
            }

            int location = glGetUniformLocation(ID, name);
            glUniform4f(location, x, y, z, w);
        }

        public void SetMatrix4x4(string uniformName, Matrix4x4 mat, bool useShader)
        {
            if (useShader)
            {
                Use();
            }

            int location = glGetUniformLocation(ID, uniformName);
            glUniformMatrix4fv(location, 1, false, GetMatrix4x4Values(mat));
        }

        unsafe void checkCompileErrors(uint obj, string type)
        {
            if (type != "PROGRAM")
            {
                int[] status = glGetShaderiv(obj, GL_COMPILE_STATUS, 1);

                if (status[0] == 0)
                {
                    //error
                    string error = glGetShaderInfoLog(obj);
                    Console.WriteLine("ERROR COMPILING VERTEX SHADER: " + error);
                }
            }
        }

        private float[] GetMatrix4x4Values(Matrix4x4 m)
        {
            return new float[]
            {
        m.M11, m.M12, m.M13, m.M14,
        m.M21, m.M22, m.M23, m.M24,
        m.M31, m.M32, m.M33, m.M34,
        m.M41, m.M42, m.M43, m.M44
            };
        }
    }
}
