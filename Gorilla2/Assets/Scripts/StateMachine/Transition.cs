using System;

namespace Platformer {
    public class Transition : ITransition {
        public IState To { get; }
        public IPredicate condition { get; }
        public Action action { get; }

        public Transition(IState to, IPredicate condition) {
            To = to;
            this.condition = condition;
        }
    }
}