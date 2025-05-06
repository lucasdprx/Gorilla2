using Platformer;
using UnityEngine;

namespace Player.PlayerStates
{
    public class CrouchState : BaseState
    {
        public CrouchState(PlayerController player) : base(player) { }

        public override void OnEnter()
        {
            base.OnEnter();
            //Debug.Log("Entering Crouch State");
            player.SetCurrentSpeed(player.crouchSpeed);
        }

        public override void Update()
        {
            base.Update();
            player.Move();
        }
    }
}