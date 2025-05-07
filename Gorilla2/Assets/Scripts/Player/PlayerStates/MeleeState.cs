using Platformer;
using UnityEngine;

namespace Player.PlayerStates
{
    public class MeleeState : BaseState
    {
        public MeleeState(PlayerController player) : base(player) { }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log($"Entering Melee Combo State with combo count");
            player.Attack();
            player.SetCurrentSpeed(player.crouchSpeed);
        }

        public override void Update()
        {
            base.Update();
            player.Move();
        }
    }
}