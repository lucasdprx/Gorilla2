namespace Player
{
    public abstract class PlayerState
    {
        public abstract void Enter(PlayerStateMachine context);

        public abstract void Update(PlayerStateMachine context);
    }
}