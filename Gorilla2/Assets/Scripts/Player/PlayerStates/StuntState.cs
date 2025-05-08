using Platformer;
using UnityEngine;

namespace Player.PlayerStates
{
    public class StuntState : BaseState
    {
        public StuntState(PlayerController player) : base(player) { }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Entering Stunt State");
        }

        public override void Update()
        {
            base.Update();
            player.Decelerate();
        }

        public override void OnExit()
        {
            base.OnExit();
            Debug.Log("Exiting Stunt State");
            
        }
    }
}