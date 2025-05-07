using Platformer;
using UnityEngine;

namespace Player.PlayerStates
{
    public class FallState : BaseState
    {
        public FallState(PlayerController player) : base(player) { }

        public override void OnEnter()
        {
            base.OnEnter();
            //Debug.Log("Entering Fall State");
        }

        public override void Update()
        {
            base.Update();
            player.Move();
        }
    }
}