using Platformer;
using UnityEngine;

namespace Player.PlayerStates
{
    public class WalkState : BaseState
    {
        public WalkState(PlayerController player) : base(player)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            player.SetCurrentSpeed(player.walkSpeed);
        }

        public override void Update()
        {
            base.Update();
            player.Move();
        }
    }
}