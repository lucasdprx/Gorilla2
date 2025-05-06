using System;
using Player;
using UnityEngine;

namespace Platformer {
    public abstract class BaseState : IState {
        protected readonly PlayerController player;
        public event Action onStateEnter;
        
        protected const float crossFadeDuration = 0.1f;
        
        protected BaseState(PlayerController player) {
            this.player = player;
        }
        
        public virtual void OnEnter() {
            // noop
            onStateEnter?.Invoke();
        }

        public virtual void Update() {
            // noop
        }

        public virtual void FixedUpdate() {
            // noop
        }

        public virtual void OnExit() {
            // noop
        }
    }
}