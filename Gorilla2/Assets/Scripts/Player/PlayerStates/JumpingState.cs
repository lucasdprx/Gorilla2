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
            Debug.Log("Entering Jumping State");
        }

        public override void Update()
        {
            base.Update();
            player.Move();
        }
    }
}