using UnityEngine;

namespace GDD.StrategyPattern
{
    public interface IManeuverBehaviour<T>
    {
        public abstract void Maneuver(T pawn, Transform target);

        public abstract void Truce();
    }
}