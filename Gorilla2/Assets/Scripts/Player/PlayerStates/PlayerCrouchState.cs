using UnityEngine;

namespace Player.PlayerStates
{
    public class PlayerCrouchState : PlayerState
    {
        public override void Enter(PlayerStateMachine context)
        {
            Debug.Log("Crouch state");
            context.SetCurrentSpeed(context.crouchSpeed);
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
            if (context.IsMoving())
            {
                if (!context.isCrouching)
                {
                    context.ChangeState(context.playerWalkState);
                }
            }
            else
            {
                context.ChangeState(context.playerIdleState);
            }
        }
    }
}