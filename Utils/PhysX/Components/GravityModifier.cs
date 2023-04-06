using _2DGameMaker.Game;
using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Scripting;
using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;

namespace _2DGameMaker.Utils.PhysX.Components
{
    public class GravityModifier : ObjectAppendedScript
    {
        readonly float gravityIntensity;
        readonly float terminalVelocity;

        float G = 0f;

        public GravityModifier(GameObject gameObject, string arg0, string arg1)
            : base(gameObject)
        {
            gravityIntensity = float.Parse(arg0);
            terminalVelocity = float.Parse(arg1);
        }

        public GravityModifier(GameObject gameObject, float gravityIntensity, float terminalVelocity)
            : base(gameObject)
        {
            this.gravityIntensity = gravityIntensity;
            this.terminalVelocity = terminalVelocity;
        }

        protected override void destroy()
        {

        }

        protected override void init()
        {

        }

        protected override void update()
        {
            if(G < terminalVelocity)
            {
                G += gravityIntensity;
            }

            gameObject.Translate(Vec2.OneY * G);
        }
    }
}
