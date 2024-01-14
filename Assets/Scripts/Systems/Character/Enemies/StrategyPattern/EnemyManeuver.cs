using System.Collections.Generic;
using System.Linq;
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
        }

        public override void Maneuver(EnemyState pawn)
        {
            if(GM.players.Count <= 0)
                return;
            
            if (GM.playMode == PlayMode.Singleplayer)
                _player = GM.players.Keys.ElementAt(0);
            else
            {
                if (GM.players.Count > 1)
                    _player = GM.players.Keys.ElementAt(Random.Range(0, GM.players.Count));
                else
                    _player = GM.players.Keys.ElementAt(0);
            }
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