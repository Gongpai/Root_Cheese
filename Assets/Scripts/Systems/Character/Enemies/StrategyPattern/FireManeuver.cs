using System;
using System.Collections;
using GDD.Spatial_Partition;
using GDD.StrategyPattern;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public class FireManeuver : EnemyManeuver
    {
        [SerializeField] private GameObject bullet;
        private EnemySpawnBullet _enemySpawnBullet;

        public override void Start()
        {
            base.Start();
            
            _enemySpawnBullet = GetComponent<EnemySpawnBullet>();
        }

        public override void Maneuver(EnemyStateMachine pawn)
        {
            base.Maneuver(pawn);
            
            OnFire(_player);
        }

        public override void Truce()
        {
            base.Truce();
        }
        
        private void OnFire(IPawn enemy)
        {
            if (enemy != null)
            {
                print("On Maneuver");
                _coroutines.Add(StartCoroutine(Waiting(
                    () => { _coroutines.Add(StartCoroutine(Firing(_enemySystem._enemyBulletConfig.rate))); },
                    _enemySystem._enemyBulletConfig.timedelay)));
            }
        }
        
        IEnumerator Waiting(UnityAction action , float time)
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
        
        IEnumerator Firing(float time)
        {
            var instruction = new WaitForEndOfFrame();
            float time_count = time;

            while (time_count > 0)
            {
                if (time_count == time)
                {
                    ToggleFire(_enemySpawnBullet);
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
        
        public void ToggleFire(EnemySpawnBullet enemySpawnBullet)
        {
            enemySpawnBullet.bulletObjectPool.Set_GameObject = bullet;

            EnemyBulletConfig _enemyBulletConfig = _enemySystem._enemyBulletConfig;
            enemySpawnBullet.OnSpawnBullet(
                _enemyBulletConfig.bullet_spawn_distance,
                _enemyBulletConfig.bullet_power,
                _enemyBulletConfig.shot,
                _enemyBulletConfig.shotSurroundMode
                );
        }
    }
}