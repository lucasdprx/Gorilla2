using Platformer;
using UnityEngine;

namespace Player.PlayerStates
{
    public class IdleState : BaseState
    {
        public IdleState(PlayerController player) : base(player) { }

        public override void OnEnter()
        {
            base.OnEnter();
            //Debug.Log("Entering Idle State");
            player.SetCurrentSpeed(player.walkSpeed);
        }
    }
}