using System;
using UnityEngine;

namespace Player.PlayerStates
{
    public class PlayerFallState : PlayerState
    {
        public override void Enter(PlayerStateMachine context)
        {
            Debug.Log("Fall state");
            CallStateEnter();
        }

        public override void Update(PlayerStateMachine context)
        {
            context.Move();
            if (context.IsGrounded())
            {
                context.ChangeState(context.playerIdleState);
            }
            if(context.IsJumping())
            {
                context.ChangeState(context.playerJumpState);
            }
        }
    }
}