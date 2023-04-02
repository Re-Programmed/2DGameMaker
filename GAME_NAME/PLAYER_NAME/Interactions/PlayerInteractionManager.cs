using _2DGameMaker.Utils.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.GAME_NAME.PLAYER_NAME.Interactions
{
    public class PlayerInteractable 
    {
        public delegate void Interaction();
        public Interaction OnInteract;

        public readonly float Radius;
        public readonly Vec2 Position;
        
        public PlayerInteractable(float radius, Vec2 position)
        {
            Radius = radius;
            Position = position;
        }
    }

    public static class PlayerInteractionManager
    {
        static List<PlayerInteractable> interactables = new List<PlayerInteractable>();

        public static void Interaction()
        {
            foreach (PlayerInteractable pi in interactables)
            {
                if(Objects.Collisions.CollisionCheck.Distance(pi.Position, GameName.ThePlayer.GetGameObject().GetPosition()) <= pi.Radius)
                {
                    pi.OnInteract.Invoke();
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
            Vec2 pos = GameName.ThePlayer.GetGameObject().GetPosition();
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
