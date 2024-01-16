using System;
using System.Collections;
using GDD.PUN;
using GDD.Spatial_Partition;
using GDD.StrategyPattern;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public class BulletFireManeuver : EnemyManeuver
    {
        [SerializeField] protected GameObject bullet;
        [SerializeField] protected EnemyBulletConfig m_enemyBulletConfig;
        protected EnemySpawnBullet _enemySpawnBullet;
        protected PunEnemyCharacterController _punECC;
        protected bool _haspunECC;

        protected UnityAction OnFireEvent;

        public EnemySpawnBullet enemySpawnBullet
        {
            get => _enemySpawnBullet;
        }

        protected virtual void Awake()
        {
            _enemySpawnBullet = GetComponent<EnemySpawnBullet>();
            _haspunECC = TryGetComponent(out _punECC);
            
            ToggleFireToEvent();
        }

        protected virtual void ToggleFireToEvent()
        {
            if (!_haspunECC)
            {
                OnFireEvent = () => { ToggleFire(_enemySpawnBullet); };
            }
            else
            {
                OnFireEvent = () =>
                {
                    OnFireEvent = () => { ToggleFire(_enemySpawnBullet); };
                    _punECC.CallRaiseToggleFireEvent(m_enemyBulletConfig.bulletType);
                };
            }
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Maneuver(EnemyState pawn)
        {
            base.Maneuver(pawn);

            if (_player == null)
            {
                //Debug.LogError($"Enemy {gameObject.name} : target player is null");
                return;
            }

            OnFire(_player);
        }

        public override void Truce()
        {
            base.Truce();
        }
        
        public virtual void OnFire(IPawn enemy)
        {
            if (enemy == null)
            {
                return;
            }

            //print("On Maneuver");
            _coroutines.Add(StartCoroutine(Waiting(
                () => { _coroutines.Add(StartCoroutine(Firing(m_enemyBulletConfig.rate))); },
                m_enemyBulletConfig.timedelay)));
        }
        
        protected IEnumerator Waiting(UnityAction action , float time)
        {
            var instruction = new WaitForEndOfFrame();
            
            float time_count = time;
            while (time_count > 0)
            {
                time_count -= Time.deltaTime;

                if (time_count <= 0)
                {
                    action.Invoke();
                    yield break;
                }

                yield return instruction;
            }
        }
        
        protected IEnumerator Firing(float time)
        {
            var instruction = new WaitForEndOfFrame();
            float time_count = time;

            while (time_count > 0)
            {
                if (time_count == time)
                {
                    OnFireEvent?.Invoke();
                }

                time_count -= Time.deltaTime;
                //print("Time IEnumerator : " + time_count);
                
                if (time_count <= 0)
                {
                    time_count = time;
                }

                yield return instruction;
            }
        }
        
        public virtual void ToggleFire(EnemySpawnBullet enemySpawnBullet, int[] posIndex = default)
        {
            //print("Fire!!!!!!!!!!!");
        }
    }
}