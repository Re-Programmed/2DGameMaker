using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Objects.Scripting
{
    public abstract class ObjectAppendedScript
    {
        protected GameObject gameObject;

        public ObjectAppendedScript(GameObject gameObject)
        {
            this.gameObject = gameObject;
            gameObject.AppendScript(this);
            init();
            Game.Game.INSTANCE.UpdateE += update;
        }

        ~ObjectAppendedScript()
        {
            Game.Game.INSTANCE.UpdateE -= update;
            gameObject?.RemoveScript(this);
            destroy();
        }

        protected abstract void init();
        protected abstract void update();
        protected abstract void destroy();

        public virtual void OnLoad()
        {
            
        }

        public virtual void OnDisable()
        {

        }
    }
}
