using Platformer;

namespace Player.PlayerStates
{
    public class SprintState : BaseState
    {
        public SprintState(PlayerController player) : base(player) { }

        public override void OnEnter()
        {
            base.OnEnter();
            player.SetCurrentSpeed(player.sprintSpeed);
        }

        public override void Update()
        {
            base.Update();
            player.Move();
        }
    }
}