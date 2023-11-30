using System;
using UnityEngine;

namespace GDD.StrategyPattern
{
    public abstract class StrategyPattern<T> : MonoBehaviour, IManeuverBehaviour<T>
    {
        protected GameManager GM;

        public virtual void Start()
        {
            GM = GameManager.Instance;
        }

        public abstract void Maneuver(T pawn);

        public abstract void Truce();
    }
}