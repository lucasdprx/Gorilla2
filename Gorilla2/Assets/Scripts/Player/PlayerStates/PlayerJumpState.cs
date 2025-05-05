using UnityEngine;

namespace Player.PlayerStates
{
    public class PlayerJumpState : PlayerState
    {
        public override void Enter(PlayerStateMachine context)
        {
            Debug.Log("Jump state");
        }

        public override void Update(PlayerStateMachine context)
        {
            context.Move();
            if (context.IsGrounded())
            {
                context.ChangeState(context.playerIdleState);
                return;
            }
            if (context.IsFalling())
            {
                context.ChangeState(context.playerFallState);
            }
        }
    }
}