using UnityEngine;

namespace Player.PlayerStates
{
    public class PlayerWalkState : PlayerState
    {
        public override void Enter(PlayerStateMachine context)
        {
            Debug.Log("Walk state");
            context.SetCurrentSpeed(context.walkSpeed);
        }

        public override void Update(PlayerStateMachine context)
        {
            context.Move();
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
            if (!context.IsMoving())
            {
                context.ChangeState(context.playerIdleState);
                return;
            }
            if(context.IsMoving())
            {
                if (context.isRunning)
                {
                    context.ChangeState(context.playerRunState);
                    return;
                }

                if (context.isCrouching)
                {
                    context.ChangeState(context.playerCrouchState);
                }
            }
        }

        
    }
}