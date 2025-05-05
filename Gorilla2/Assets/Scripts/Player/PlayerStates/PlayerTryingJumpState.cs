using UnityEngine;

namespace Player.PlayerStates
{
    public class PlayerTryingJumpState : PlayerState
    {
        private float jumpTime;
        public override void Enter(PlayerStateMachine context)
        {
            Debug.Log("Trying Jump state");
            CallStateEnter();
            jumpTime = Time.time;
        }

        public override void Update(PlayerStateMachine context)
        {
            //TODO: if()
        }
    }
}