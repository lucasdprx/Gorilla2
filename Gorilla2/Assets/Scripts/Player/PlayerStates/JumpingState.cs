using Platformer;
using UnityEngine;

namespace Player.PlayerStates
{
    public class JumpingState : BaseState
    {
        public JumpingState(PlayerController player) : base(player) { }

        public override void OnEnter()
        {
            base.OnEnter();
            player.Jump();
        }

        public override void Update()
        {
            base.Update();
            player.Move();
        }

        public override void OnExit()
        {
            base.OnExit();
            player.StopJump();
        }
    }
}