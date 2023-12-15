using System.Collections.Generic;
using GDD.StrategyPattern;
using UnityEngine;
using UnityEngine.Serialization;

namespace GDD
{
    public class EnemyManeuver : StrategyPattern<EnemyState>
    {
        [SerializeField] protected float m_bulletSpawnDistance = 0.25f;
        protected List<Coroutine> _coroutines = new List<Coroutine>();
        protected PlayerSystem _player;
        protected EnemySystem _enemySystem;

        public virtual void Start()
        {
            base.Start();
            
            _enemySystem = GetComponent<EnemySystem>();
            if(GM.playMode == PlayMode.Singleplayer)
                _player = GM.players[0];
        }

        public override void Maneuver(EnemyState pawn)
        {
            
        }

        public override void Truce()
        {
            //print("On Truce!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            foreach (var coroutine in _coroutines)
            {
                if(coroutine != null)
                    StopCoroutine(coroutine);
            }

            _coroutines = new List<Coroutine>();
        }
    }
}