using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Scripting;
using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.GAME_NAME.PLAYER_NAME.Interactions
{
    public abstract class PlayerInteractable : ObjectAppendedScript
    {
        public abstract void OnInteract(PlayerName player);

        public readonly float Radius;
        public readonly Vec2 Position;

        public readonly GLFW.Keys Key;

        public readonly bool SingleInteraction = false;
        public bool Interacted = false;

        public PlayerInteractable(GameObject gameObject, GLFW.Keys key, bool singleInteraction = false)
            : base(gameObject)
        {
            Radius = gameObject.GetScale().X / 2;
            Position = gameObject.GetCenter();
            Key = key;
            SingleInteraction = singleInteraction;

            PlayerInteractionManager.LoadInteractable(this);
        }

        public PlayerInteractable(GameObject gameObject, int key, bool singleInteraction = false)
           : base(gameObject)
        {
            Radius = gameObject.GetScale().X / 2;
            Position = gameObject.GetCenter();
            Key = (GLFW.Keys)key;
            SingleInteraction = singleInteraction;

            PlayerInteractionManager.LoadInteractable(this);
        }

        public PlayerInteractable(GameObject gameObject, string key)
           : base(gameObject)
        {
            Radius = gameObject.GetScale().X / 2;
            Position = gameObject.GetCenter();
            Key = (GLFW.Keys)int.Parse(key);

            PlayerInteractionManager.LoadInteractable(this);
        }

        public PlayerInteractable(GameObject gameObject, float radius, GLFW.Keys key)
            : base(gameObject)
        {
            Radius = radius;
            Position = gameObject.GetCenter();
            Key = key;

            PlayerInteractionManager.LoadInteractable(this);
        }

        public PlayerInteractable(GameObject gameObject, float radius, Vec2 position, GLFW.Keys key)
            : base(gameObject)   
        {
            Radius = radius;
            Position = position;
            Key = key;

            PlayerInteractionManager.LoadInteractable(this);
        }

        public PlayerInteractable(GameObject gameObject, string radius, string positionX, string positionY, GLFW.Keys key)
            : base(gameObject)
        {
            Radius = float.Parse(radius);
            Position = new Vec2(float.Parse(positionX), float.Parse(positionY));
            Key = key;

            PlayerInteractionManager.LoadInteractable(this);
        }
    }

    public static class PlayerInteractionManager
    {
        static List<PlayerInteractable> interactables = new List<PlayerInteractable>();

        public static void CheckInteraction()
        {
            foreach (PlayerInteractable pi in interactables)
            {
                if (!Input.Input.GetKey(pi.Key))
                {
                    if (pi.Interacted) { pi.Interacted = false; }
                    continue;
                }

                if (Objects.Collisions.CollisionCheck.Distance(pi.Position, GameName.ThePlayer.GetGameObject().GetCenter()) <= pi.Radius)
                {
                    if (pi.SingleInteraction)
                    {
                        if (!pi.Interacted)
                        {
                            pi.OnInteract(GameName.ThePlayer);
                            pi.Interacted = true;
                        }
                    }
                    else
                    {
                        pi.OnInteract(GameName.ThePlayer);
                    }
                }
            }
        }

        public static void LoadInteractable(PlayerInteractable i)
        {
            interactables.Add(i);
        }

        public static void RemoveInteractable(PlayerInteractable i)
        {
            interactables.Remove(i);
        }

        public static bool PlayerInBounds(float x, float y, Vec2 location)
        {
            Vec2 pos = GameName.ThePlayer.GetGameObject().GetCenter();
            if(pos.X < location.X + x && pos.X > location.X)
            {
                if (pos.Y > location.Y && pos.Y < location.Y + y)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
