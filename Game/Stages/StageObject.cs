using System;
using System.Collections.Generic;
using System.Text;
using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Scripting;
using _2DGameMaker.Objects.Stationaries;
using _2DGameMaker.Utils.AssetManagment;
using _2DGameMaker.Utils.Math;

namespace _2DGameMaker.Game.Stages
{
    public struct StageObject
    {
        public int Layer;

        public float[] Position;
        public float[] Scale;
        public float Rotation;

        public string[] Texture;

        public StageComponent[] Components;

        public StageObject(GameObject obj, string texture, string dir)
        {
            Layer = 1;
            Position = obj.GetLocalPosition().GetArray();
            Scale = obj.GetLocalScale().GetArray();
            Rotation = obj.GetLocalRotation();

            Components = new StageComponent[obj.ObjectAppenedScripts.Count];
            int i = 0;
            foreach (ObjectAppendedScript oas in obj.ObjectAppenedScripts)
            {
                Components[i] = new StageComponent(oas.GetType().ToString(), new string[0]);
                i++;
            }

            Texture = new string[2] { texture, dir };
        }

        public StageObject(GameObject obj)
        {
            Layer = 1;
            Position = obj.GetLocalPosition().GetArray();
            Scale = obj.GetLocalScale().GetArray();
            Rotation = obj.GetLocalRotation();

            Texture = new string[2] { "TEMPLATE", "TEMPLATE_DIR" };

            Components = new StageComponent[obj.ObjectAppenedScripts.Count];
            int i = 0;
            foreach (ObjectAppendedScript oas in obj.ObjectAppenedScripts)
            {
                Components[i] = new StageComponent(oas.GetType().ToString(), new string[0]);
                i++;
            }
        }

        public StageObject(StageObject so)
        {
            Layer = so.Layer;
            Position = so.Position;
            Scale = so.Scale;
            Rotation = so.Rotation;
            Texture = so.Texture;
            Components = so.Components;
        }

        public StaticObject GetObject()
        {
            ObjectTexture texture = new ObjectTexture(AssetManager.GetTexture(Texture[0], Texture[1]));
            StaticObject so = new StaticObject(new Vec2(Position[0], Position[1]), Scale.Length == 2 ? new Vec2(Scale[0], Scale[1]) : new Vec2(texture.GetTexture().Width, texture.GetTexture().Height), Rotation, texture);

            if (Components != null)
            {
                foreach (StageComponent comp in Components)
                {
                    Type t = Type.GetType(comp.Name);
                    object[] args = new object[comp.Args.Length + 1];

                    for (int i = 1; i <= comp.Args.Length; i++)
                    {
                        args[i] = comp.Args[i - 1];
                    }

                    args[0] = so;

                    ObjectAppendedScript inst = (ObjectAppendedScript)Activator.CreateInstance(t, args);
                    so.AppendScript(inst);
                }
            }

            return so;
        }
    }
}
