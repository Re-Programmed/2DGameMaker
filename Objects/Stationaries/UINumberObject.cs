using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Objects.Stationaries
{
    /// <summary>
    /// A GameObject for numbers drawn to the screen.
    /// </summary>
    public class UINumberObject : GameObject
    {
        #region Constructors
        public UINumberObject(Vec2 position, Vec2 scale, ObjectTexture texture, GameObject parent = null)
            : base(position, scale, 0f, texture, true, parent, true)
        { }

        public UINumberObject(Vec2 position, ObjectTexture texture, GameObject parent = null)
            : base(position, texture, true, parent, true)
        { }


        #endregion

    }
}
