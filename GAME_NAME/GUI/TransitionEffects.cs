using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Stationaries;
using _2DGameMaker.Rendering.Display;
using _2DGameMaker.Utils.AssetManagment;
using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace _2DGameMaker.GAME_NAME.GUI
{
    public static class TransitionEffects
    {
        #region Circle Shrink Effect
        /// <summary>
        /// The layer in which the circle objects of this effect should be instantiated.
        /// </summary>
        const int CIRCLE_LAYER = 3;

        /// <summary>
        /// Data for the threads that allow the circle shrinking effect to run.
        /// </summary>
        public struct CircleEffectData
        {
            /// <summary>
            /// The circle object.
            /// </summary>
            public GameObject Circle;
            /// <summary>
            /// How long to delay the thread before start.
            /// </summary>
            public int Delay;
            /// <summary>
            /// If the circle should be removed.
            /// </summary>
            public bool Destroy;

            public CircleEffectData(GameObject circle, int delay, bool destroy)
            {
                Circle = circle;
                Delay = delay;
                Destroy = destroy;
            }
        }

        /// <summary>
        /// Plays the effect of multiple colored circles shrinking or growing on the screen.
        /// </summary>
        /// <param name="destroy">If true, destroys the created circles after the effect has played.</param>
        /// <param name="grow">Specifies if the circle effect should grow or shrink.</param>
        /// <returns>The circle objects created.</returns>
        public static GameObject[] MultiCircleEffect(bool destroy = true, bool grow = false)
        {
            StaticObject red = new StaticObject(Vec2.Zero, grow ? Vec2.One : Vec2.One * DisplayManager.WindowSize.X * 2, 0f, new ObjectTexture(AssetManager.GetTexture(grow ? "blue_circle" : "red_circle", "general_assets")));
            red.SetCenter(Vec2.Zero);
            StaticObject green = new StaticObject(Vec2.Zero, grow ? Vec2.One : Vec2.One * DisplayManager.WindowSize.X * 2, 0f, new ObjectTexture(AssetManager.GetTexture(grow ? "yellow_circle" : "green_circle", "general_assets")));
            green.SetCenter(Vec2.Zero);
            StaticObject yellow = new StaticObject(Vec2.Zero, grow ? Vec2.One : Vec2.One * DisplayManager.WindowSize.X * 2, 0f, new ObjectTexture(AssetManager.GetTexture(grow ? "green_circle" : "yellow_circle", "general_assets")));
            yellow.SetCenter(Vec2.Zero);
            StaticObject blue = new StaticObject(Vec2.Zero, grow ? Vec2.One : Vec2.One * DisplayManager.WindowSize.X * 2, 0f, new ObjectTexture(AssetManager.GetTexture(grow ? "red_circle" : "blue_circle", "general_assets")));
            blue.SetCenter(Vec2.Zero);

            if(grow)
            {
                Game.Game.INSTANCE.Instantiate(red, CIRCLE_LAYER);
                Game.Game.INSTANCE.Instantiate(green, CIRCLE_LAYER);
                Game.Game.INSTANCE.Instantiate(yellow, CIRCLE_LAYER);
                Game.Game.INSTANCE.Instantiate(blue, CIRCLE_LAYER);
            }
            else
            {
                Game.Game.INSTANCE.Instantiate(blue, CIRCLE_LAYER);
                Game.Game.INSTANCE.Instantiate(yellow, CIRCLE_LAYER);
                Game.Game.INSTANCE.Instantiate(green, CIRCLE_LAYER);
                Game.Game.INSTANCE.Instantiate(red, CIRCLE_LAYER);
            }

            shrinkRate = SHRINK_SPEED * (grow?-1:1);

            Thread t_redShrink = new Thread(shrinkCircle);
            t_redShrink.Start(new CircleEffectData(red, 0, destroy));

            Thread t_greenShrink = new Thread(shrinkCircle);
            t_greenShrink.Start(new CircleEffectData(green, 150, destroy));

            Thread t_yellowShrink = new Thread(shrinkCircle);
            t_yellowShrink.Start(new CircleEffectData(yellow, 300, destroy));

            Thread t_blueShrink = new Thread(shrinkCircle);
            t_blueShrink.Start(new CircleEffectData(blue, 450, destroy));

            return new GameObject[] { red, green, yellow, blue };

        }

        /// <summary>
        /// The speed at which to lerp the cirlces sizes.
        /// </summary>
        readonly static float SHRINK_SPEED = 0.000002f;
        /// <summary>
        /// Changes to negative or positive based on if the circles should grow or shrink.
        /// </summary>
        static float shrinkRate;

        /// <summary>
        /// Runs on a separate thread to allow for multiple while loops to shrink circles.
        /// </summary>
        /// <param name="go">The CircleEffectData.</param>
        private static void shrinkCircle(object go)
        {
            CircleEffectData g = (CircleEffectData)go;

            Thread.Sleep(g.Delay);

            while(shrinkRate > 0f ? g.Circle.GetScale().X > 1f : g.Circle.GetScale().X < DisplayManager.WindowSize.X * 2f)
            {
                g.Circle.SetCenter(Vec2.Zero);

                float s = _2DGameMaker.Utils.Math.Math.Lerp(g.Circle.GetScale().X, 0.5f, shrinkRate);

                g.Circle.SetScale(Vec2.One * s);
               
            }

            if (g.Destroy) { Game.Game.INSTANCE.Destroy(g.Circle, CIRCLE_LAYER); }

        }
        #endregion

    }
}
