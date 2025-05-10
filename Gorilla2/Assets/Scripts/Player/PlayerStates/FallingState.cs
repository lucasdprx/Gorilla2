using Platformer;

namespace Player.PlayerStates
{
    public class FallingState : BaseState
    {
        public FallingState(PlayerController player) : base(player) { }

        public override void Update()
        {
            base.Update();
            player.Move();
        }
    }
}