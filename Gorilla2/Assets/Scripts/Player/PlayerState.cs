using System;

namespace Player
{
    public abstract class PlayerState
    {
        public event Action OnStateEnter;
        
        protected void CallStateEnter()
        {
            OnStateEnter?.Invoke();
        }
        public abstract void Enter(PlayerStateMachine context);

        public abstract void Update(PlayerStateMachine context);
    }
}