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
        #region Physics Params
        private const float SPEED = 0.05f;
        private const float SPEED_DECREMENT = 0.0005f;
        private const float SPEED_CAP = 0.25f;

        private const float JUMP_TARGET = -.4f;
        private const float AIR_RESISTANCE = 0.35f;
        private const float JUMP_SMOOTHING = 2000;

        private const float AIR_SPEED = 0.0001f;
        #endregion

        private PlayerStats playerStats;

        public bool IsClimbing { get; private set; } = false;
        private bool atLadderCap = false;

        public PlayerName(GameObject gameObject, string arg0)
            : base(gameObject)
        {
            GameName.ThePlayer = this;
            playerStats = GameName.GetSavedPlayerStats();
        }

        public void SetClimbing(bool climbing, bool cap = false)
        {
            IsClimbing = climbing;
            gravity.SetEnabled(!IsClimbing);
            allowJump = false;
            atLadderCap = cap;
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
            //Why is the velocity set to 156 on start?
            if (motionVector.X > 155) { motionVector.SetX(0); }
            updateMovement();
        }

        Vec2 motionVector = Vec2.Zero;
        Vec2 jump = Vec2.Zero;
        private void updateMovement()
        {
            if (IsClimbing)
            {
                Console.WriteLine(IsClimbing);
                if (Input.Input.GetKey(ControlsManager.GetKey(ControlsManager.ControlOption.PlayerMove_UP)))
                {
                    if (!atLadderCap)
                    {
                        gameObject.Translate(-Vec2.OneY / 10 * GameTime.NormalizedDeltaTime());
                    }
                }

                if (Input.Input.GetKey(ControlsManager.GetKey(ControlsManager.ControlOption.PlayerMove_DOWN)))
                {
                    gameObject.Translate(Vec2.OneY/10 * GameTime.NormalizedDeltaTime());
                    atLadderCap = false;
                }

                if(Input.Input.GetKey(ControlsManager.GetKey(ControlsManager.ControlOption.PlayerMove_RIGHT)))
                {
                    jump = Input.Input.GetKey(ControlsManager.GetKey(ControlsManager.ControlOption.PlayerMove_UP)) ? Vec2.OneY * JUMP_TARGET : Vec2.Zero;
                    motionVector = new Vec2(SPEED_CAP, jump.Y);
                    SetClimbing(false);
                }

                if (Input.Input.GetKey(ControlsManager.GetKey(ControlsManager.ControlOption.PlayerMove_LEFT)))
                {
                    jump = Input.Input.GetKey(ControlsManager.GetKey(ControlsManager.ControlOption.PlayerMove_UP)) ? Vec2.OneY * JUMP_TARGET : Vec2.Zero;
                    motionVector = new Vec2(-SPEED_CAP, jump.Y);
                    SetClimbing(false);
                }
                return;
            }
            bool movedOrTerminalVelocity = false;

            if (Input.Input.GetKey(ControlsManager.GetKey(ControlsManager.ControlOption.PlayerMove_LEFT)))
            {
                movedOrTerminalVelocity = true;
                if (motionVector.X > -SPEED_CAP)
                {
                    motionVector += Vec2.OneX * -(allowJump ? SPEED : AIR_SPEED);
                }
            }

            if (Input.Input.GetKey(ControlsManager.GetKey(ControlsManager.ControlOption.PlayerMove_RIGHT)))
            {
                movedOrTerminalVelocity = true;
                if (motionVector.X < SPEED_CAP)
                {
                    motionVector += Vec2.OneX * (allowJump ? SPEED : AIR_SPEED);
                }
            }

            if (!movedOrTerminalVelocity)
            {
                if (motionVector.X > 0.1f)
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

            gameObject.Translate(new Vec2(motionVector.X * GameTime.NormalizedDeltaTime(), motionVector.Y));

            if (motionVector.Y != 0 && jump.Y == 0)
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
            if (allowJump)
            {
                if (Input.Input.GetKey(ControlsManager.GetKey(ControlsManager.ControlOption.PlayerMove_JUMP)))
                {
                    allowJump = false;

                    jump = Vec2.OneY * JUMP_TARGET;
                    motionVector.SetY(jump.Y);

                    playerStats.AddScore(5);
                }
            }
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public override void OnLoad() { }

        public override void OnDisable() { }
    }
}
