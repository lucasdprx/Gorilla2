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

        public override void Update()
        {
            base.Update();
            player.rb.linearVelocity = new Vector2(Mathf.Lerp(player.rb.linearVelocity.x, 0, Time.deltaTime * player.decelerationForce), player.rb.linearVelocity.y);
        }
    }
}