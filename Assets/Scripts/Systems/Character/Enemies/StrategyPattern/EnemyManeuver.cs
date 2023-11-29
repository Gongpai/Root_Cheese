using System.Collections.Generic;
using GDD.StrategyPattern;
using UnityEngine;

namespace GDD
{
    public class EnemyManeuver : StrategyPattern<EnemyStateMachine>
    {
        protected List<Coroutine> _coroutines = new List<Coroutine>();
        
        public override void Maneuver(EnemyStateMachine pawn)
        {
            
        }

        public override void Truce()
        {
            
        }
    }
}