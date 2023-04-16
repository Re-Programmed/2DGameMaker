using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.Objects.Scripting
{
    public abstract class ObjectAppendedScript
    {
        protected GameObject gameObject;
        public bool IsEnabled { get; private set; } = true;

        public void SetEnabled(bool enabled)
        {
            IsEnabled = enabled;
        }

        /// <summary>
        /// Constructor to add to the update loop.
        /// </summary>
        /// <param name="gameObject"></param>
        public ObjectAppendedScript(GameObject gameObject)
        {
            this.gameObject = gameObject;
            gameObject.AppendScript(this);
            init();
            Game.Game.INSTANCE.UpdateE += preUpdate;
        }

        /// <summary>
        /// Destructor to remove from the update loop.
        /// </summary>
        ~ObjectAppendedScript()
        {
            Game.Game.INSTANCE.UpdateE -= preUpdate;
            gameObject?.RemoveScript(this);
            destroy();
        }

        protected abstract void init();
        protected abstract void update();
        protected abstract void destroy();

        private void preUpdate()
        {
            if (!IsEnabled) { return; }
            update();
        }

        bool inDelegate = true;
        /// <summary>
        /// Make sure to use base.OnLoad() to allow the object to disable off screen.
        /// </summary>
        public virtual void OnLoad()
        {
            if (!inDelegate) { Game.Game.INSTANCE.UpdateE += preUpdate; inDelegate = true; }
        }

        /// <summary>
        /// Make sure to use base.OnDisable() to allow the object to disable off screen.
        /// </summary>
        public virtual void OnDisable()
        {
            if (inDelegate) { Game.Game.INSTANCE.UpdateE -= preUpdate; inDelegate = false; }
        }

        /// <summary>
        /// Returns a copy of this script. Make sure to add args if you inherit this class.
        /// </summary>
        /// <returns></returns>
        public virtual ObjectAppendedScript Clone(GameObject gameObject)
        {
            return (ObjectAppendedScript)Activator.CreateInstance(GetType(), gameObject);
        }
    }
}
