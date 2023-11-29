namespace GDD.StrategyPattern
{
    public interface IManeuverBehaviour<T>
    {
        public abstract void Maneuver(T pawn);

        public abstract void Truce();
    }
}