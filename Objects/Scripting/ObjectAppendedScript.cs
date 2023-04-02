using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Objects.Scripting
{
    public abstract class ObjectAppendedScript
    {
        protected GameObject gameObject;

        /// <summary>
        /// Constructor to add to the update loop.
        /// </summary>
        /// <param name="gameObject"></param>
        public ObjectAppendedScript(GameObject gameObject)
        {
            this.gameObject = gameObject;
            gameObject.AppendScript(this);
            init();
            Game.Game.INSTANCE.UpdateE += update;
        }

        /// <summary>
        /// Destructor to remove from the update loop.
        /// </summary>
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

        /// <summary>
        /// Returns a copy of this script. Make sure to add args if you inherit this class.
        /// </summary>
        /// <returns></returns>
        public virtual ObjectAppendedScript Clone()
        {
            return (ObjectAppendedScript)Activator.CreateInstance(GetType());
        }
    }
}
