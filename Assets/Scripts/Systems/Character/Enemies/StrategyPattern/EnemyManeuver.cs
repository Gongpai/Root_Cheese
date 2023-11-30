using System.Collections.Generic;
using GDD.StrategyPattern;
using UnityEngine;

namespace GDD
{
    public class EnemyManeuver : StrategyPattern<EnemyStateMachine>
    {
        protected List<Coroutine> _coroutines = new List<Coroutine>();
        protected PlayerSystem _player;

        public virtual void Start()
        {
            base.Start();
            
            if(GM.playMode == PlayMode.Singleplayer)
                _player = GM.players[0];
        }

        public override void Maneuver(EnemyStateMachine pawn)
        {
            
        }

        public override void Truce()
        {
            print("On Truce!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            foreach (var coroutine in _coroutines)
            {
                StopCoroutine(coroutine);
            }
        }
    }
}