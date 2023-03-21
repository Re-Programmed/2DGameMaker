using _2DGameMaker.Game;
using _2DGameMaker.Objects;
using _2DGameMaker.Objects.Scripting;
using _2DGameMaker.Utils.Math;
using _2DGameMaker.Utils.PhysX.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DGameMaker.GAME_NAME.PLAYER_NAME
{
    public class PlayerName : ObjectAppendedScript
    {

        private const float SPEED = 0.05f;
        private const float SPEED_DECREMENT = 0.0005f;
        private const float SPEED_CAP = 0.25f;

        private const float JUMP_TARGET = -0.4f;
        private const float AIR_RESISTANCE = 0.35f;
        private const float JUMP_SMOOTHING = 2000;

        public PlayerName(GameObject gameObject, string arg0)
            : base(gameObject)
        {

        }

        protected override void destroy()
        {

        }

        DynamicCollisionObject collider;
        GravityModifier gravity;

        private void onCollision(DynamicCollisionObject.CollisionAxis axis)
        {
            if (axis == DynamicCollisionObject.CollisionAxis.TOP)
            {
                allowJump = true;
            }
        }

        protected override void init()
        {
            collider = new DynamicCollisionObject(gameObject, null);
            gameObject.AppendScript(collider);

            gravity = new GravityModifier(gameObject, 0.07f, 0.17f);
            gameObject.AppendScript(gravity);

            collider.collisionEvent += onCollision;
        }

        protected override void update()
        {
            //WTF, Why does this need to be here? Why is the velocity set to 156 on start?
            if (motionVector.X > 155) { motionVector.SetX(0); }
            updateMovement();
        }

        Vec2 motionVector = Vec2.Zero;
        Vec2 jump = Vec2.Zero;
        private void updateMovement()
        {
            bool movedOrTerminalVelocity = false;
            
            if (Input.Input.GetKey(ControlsManager.GetKey(ControlsManager.ControlOption.PlayerMove_LEFT)))
            {
                movedOrTerminalVelocity = true;
                if (motionVector.X > -SPEED_CAP)
                {
                    motionVector += Vec2.OneX * -SPEED;
                }
            }

            if (Input.Input.GetKey(ControlsManager.GetKey(ControlsManager.ControlOption.PlayerMove_RIGHT)))
            {
                movedOrTerminalVelocity = true;
                if (motionVector.X < SPEED_CAP)
                {
                    motionVector += Vec2.OneX * SPEED;
                }
            }

            if(!movedOrTerminalVelocity)
            {
                if(motionVector.X > 0.1f)
                {
                    motionVector -= Vec2.OneX * SPEED_DECREMENT;
                }
                else if (motionVector.X < -0.1f)
                {
                    motionVector += Vec2.OneX * SPEED_DECREMENT;
                }
                else
                {
                    motionVector.SetX(0);
                }
                
            }
            
            handleJumping();

            if (jump.Y != 0)
            {
                if (motionVector.Y <= 0)
                {
                    motionVector.AddY(-jump.Y / JUMP_SMOOTHING);
                }
                else
                {
                    jump.SetY(0);
                }
            }
            gameObject.Translate(motionVector);

            if(motionVector.Y != 0 && jump.Y == 0)
            {
                if (motionVector.Y < 0.1f)
                {
                    motionVector.AddY(AIR_RESISTANCE);
                }
                else
                {
                    motionVector.SetY(0);
                }
            }
        }

        private bool allowJump = false;
        private void handleJumping()
        {
            if(allowJump)
            {
                if (Input.Input.GetKey(ControlsManager.GetKey(ControlsManager.ControlOption.PlayerMove_UP)))
                {
                    allowJump = false;

                    jump = Vec2.OneY * JUMP_TARGET;
                    motionVector.SetY(jump.Y);
                }
            }
        }

    }
}
