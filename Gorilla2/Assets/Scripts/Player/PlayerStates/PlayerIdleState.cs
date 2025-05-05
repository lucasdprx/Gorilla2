using UnityEngine;

namespace Player.PlayerStates
{
    public class PlayerIdleState : PlayerState
    {
        public override void Enter(PlayerStateMachine context)
        {
            Debug.Log("Idle state");
        }

        public override void Update(PlayerStateMachine context)
        {
            if(context.IsJumping())
            {
                context.ChangeState(context.playerJumpState);
                return;
            }
            if (context.IsFalling())
            {
                context.ChangeState(context.playerFallState);
                return;
            }
            if (context.moveInput.magnitude > context.moveSpeedThreshold)
            {
                if (context.isRunning)
                {
                    context.ChangeState(context.playerRunState);
                    return;
                }

                if (context.isCrouching)
                {
                    context.ChangeState(context.playerCrouchState);
                    return;
                }
                context.ChangeState(context.playerWalkState);
            }
        }
    }
}