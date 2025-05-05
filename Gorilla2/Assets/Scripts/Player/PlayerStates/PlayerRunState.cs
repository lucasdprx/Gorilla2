using UnityEngine;

namespace Player.PlayerStates
{
    public class PlayerRunState : PlayerState
    {
        public override void Enter(PlayerStateMachine context)
        {
            Debug.Log("Run state");
            CallStateEnter();
            context.SetCurrentSpeed(context.runSpeed);
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
            }
            else
            {
                if (!context.isRunning)
                {
                    context.ChangeState(context.playerWalkState);
                }
            }
        }
    }
}