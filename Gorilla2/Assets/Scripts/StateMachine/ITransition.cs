namespace Platformer {
    public interface ITransition {
        IState To { get; }
        IPredicate condition { get; }
    }
}