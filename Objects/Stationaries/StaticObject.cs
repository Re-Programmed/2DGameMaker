using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Objects.Stationaries
{
    public class StaticObject : GameObject
    {
        #region Constructors
        public StaticObject(Vec2 position, Vec2 scale, float rotation, ObjectTexture texture, bool alwaysLoad = false, GameObject parent = null, bool isUI = false)
            :base(position, scale, rotation, texture, alwaysLoad, parent, isUI)
        { }

        public StaticObject(Vec2 position, float rotation, ObjectTexture texture, bool alwaysLoad = false, GameObject parent = null, bool isUI = false)
            : base(position, rotation, texture, alwaysLoad, parent, isUI)
        { }

        public StaticObject(Vec2 position, ObjectTexture texture, bool alwaysLoad = false, GameObject parent = null, bool isUI = false)
            : base(position, texture, alwaysLoad, parent, isUI)
        { }


        #endregion

    }
}
