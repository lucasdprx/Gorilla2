using Platformer;
using UnityEngine;

namespace Player.PlayerStates
{
    public class MeleeState : BaseState
    {
        private float hitTime;

        public MeleeState(PlayerController player) : base(player)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            player.SetCurrentSpeed(player.crouchSpeed);
            Hit();
        }

        private void Hit()
        {
            hitTime = Time.time;
            player.Attack();
        }

        public override void Update()
        {
            base.Update();
            player.Move();
            bool canCombo = hitTime + player.comboWindow + player.attackCooldown> Time.time;
            bool attackOnCooldown = hitTime + player.attackCooldown >= Time.time;
            if (canCombo)
            {
                if (!attackOnCooldown && player.isInputsPressed[InputManager.InputActionType.Attack])
                {
                    Hit();
                }
            }
            else
            {
                player.ResetAttack();
            }
        }
    }
}