using _2DGameMaker.Rendering.Display;
using GLFW;
using System;
using System.Collections.Generic;
using System.Numerics;
using _2DGameMaker.Objects;
using _2DGameMaker.Utils;
using System.Text;
using static _2DGameMaker.OpenGL.GL;
using _2DGameMaker.Utils.Math;

namespace _2DGameMaker.Rendering.Sprites
{
    class SpriteRenderer
    {
        private static SpriteShader shader;

        static readonly Matrix4x4 FlipMat = Matrix4x4.CreateRotationY(_2DGameMaker.Utils.Math.Math.DegToRad(180));

        public static float[] quadTextureVerts = { 
        // pos      // tex
        0.0f, 0.0f, 0.0f, 0.0f,
        0.0f, 1.0f, 0.0f, 1.0f,
        1.0f, 0.0f, 1.0f, 0.0f,

        0.0f, 1.0f, 0.0f, 1.0f,
        1.0f, 1.0f, 1.0f, 1.0f,
        1.0f, 0.0f, 1.0f, 0.0f
        };

        public static uint quadVAO;

        public static void InitShader(SpriteShader shader)
        {
            SpriteRenderer.shader = shader;
        }

        public SpriteRenderer()
        {

        }

        public static void DrawSprite(Cameras.Camera2d cam, GameObject obj, Texture2D texture, Vec3 color, bool UI = false)
        {
            Matrix4x4 trans;
            Matrix4x4 sca;
            Matrix4x4 rot = Matrix4x4.CreateRotationZ(obj.GetRotation());

            if(UI)
            {
                trans = Matrix4x4.CreateTranslation(obj.GetPosition().X / cam.Zoom + cam.FocusPosition.X, obj.GetPosition().Y / cam.Zoom + cam.FocusPosition.Y, 0);
                sca = Matrix4x4.CreateScale(obj.GetScale().X / cam.Zoom, obj.GetScale().Y / cam.Zoom, 1);
            }
            else
            {
                trans = Matrix4x4.CreateTranslation(obj.GetPosition().X + (obj.Texture.GetFlipped() ? obj.GetScale().X : 0), obj.GetPosition().Y, 0);
                sca = Matrix4x4.CreateScale(obj.GetScale().X, obj.GetScale().Y, 1);
            }

            if (obj.Texture.GetFlipped()) { shader.SetMatrix4x4("model", sca * rot * trans * FlipMat, false); } else { shader.SetMatrix4x4("model", sca * rot * trans, false); }

            shader.Use();
            shader.SetMatrix4x4("projection", cam.GetProjectionMatrix(), false);
            shader.SetVector3f("spriteColor", color.GetVector(), false);

            glActiveTexture(GL_TEXTURE0);
            texture.Bind();

            glDrawArrays(GL_TRIANGLES, 0, quadTextureVerts.Length);
        }

        public void DrawSprite(Cameras.Camera2d cam, Vector2 pos, Vector2 scale, float rotation, float[] vertices, Texture2D texture, Vector3 color)
        {
            Matrix4x4 trans = Matrix4x4.CreateTranslation(pos.X, pos.Y, 0);
            Matrix4x4 sca = Matrix4x4.CreateScale(scale.X, scale.Y, 1);
            Matrix4x4 rot = Matrix4x4.CreateRotationZ(rotation);

            shader.SetMatrix4x4("model", sca * rot * trans, false);

            shader.Use();
            shader.SetMatrix4x4("projection", cam.GetProjectionMatrix(), false);
            shader.SetVector3f("spriteColor", color, false);

            glActiveTexture(GL_TEXTURE0);
            texture.Bind();

            glDrawArrays(GL_TRIANGLES, 0, vertices.Length);
        }

        public static unsafe void InitRenderData()
        {
            uint VBO = 0;
            quadVAO = glGenVertexArray();
            VBO = glGenBuffer();

            glBindBuffer(GL_ARRAY_BUFFER, VBO);

            fixed (float* v = &quadTextureVerts[0])
            {
                glBufferData(GL_ARRAY_BUFFER, sizeof(float) * quadTextureVerts.Length, v, GL_STATIC_DRAW);
            }

            glBindVertexArray(quadVAO);

            glEnableVertexAttribArray(0);
            glVertexAttribPointer(0, 4, GL_FLOAT, false, 4 * sizeof(float), (void*)0);
            glBindBuffer(GL_ARRAY_BUFFER, 0);
            glBindVertexArray(0);
        }
    }
}
