using UnityEngine;

namespace GDD.StrategyPattern
{
    public abstract class StrategyPattern<T> : MonoBehaviour, IManeuverBehaviour<T>
    {
        public abstract void Maneuver(T pawn);

        public abstract void Truce();
    }
}