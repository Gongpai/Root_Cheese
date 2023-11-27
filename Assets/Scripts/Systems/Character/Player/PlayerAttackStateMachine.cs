using System;
using System.Collections;
using System.Collections.Generic;
using GDD.Spatial_Partition;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public class PlayerAttackStateMachine : PlayerStateMachine
    {
        private SpawnBullet _spawnBullet;
        private WeaponSystem _weaponSystem;
        private bool _is_Start_Fire = false;
        private List<Coroutine> _coroutinesFires = new List<Coroutine>();

        protected override void Start()
        {
            base.Start();
            
            _spawnBullet = GetComponent<SpawnBullet>();
            _weaponSystem = GetComponent<WeaponSystem>();
        }

        public override string StateName()
        {
            return "PlayerAttackState";
        }
        
        public override void OnStart(PlayerSystem contrller)
        {
            base.OnStart(contrller);

            LookAtEnemy();

            _is_Start_Fire = true;
            
            foreach (var coroutine in _coroutinesFires)
            {
                StopCoroutine(coroutine);
            }
            _coroutinesFires = new List<Coroutine>();
            
            _coroutinesFires.Add(StartCoroutine(Waiting(() =>
            {
                _coroutinesFires.Add(StartCoroutine(OnFire(_weaponSystem.Get_Weapon.rate)));
            }, _playerSystem.delay_attack)));
        }

        public override void Handle(PlayerSystem contrller)
        {
            base.Handle(contrller);
        }

        public override void OnExit()
        {
            base.OnExit();

            foreach (var coroutine in _coroutinesFires)
            {
                StopCoroutine(coroutine);
            }
            
            _is_Start_Fire = false;
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
                    //print("End Time!!!!!!!!!!!!!!!!!!!");
                    if (_isEnterState)
                        action.Invoke();
                    
                    yield break;
                }

                yield return instruction;
            }
        }
        
        IEnumerator OnFire(float time)
        {
            var instruction = new WaitForEndOfFrame();
            float time_count = time;

            while (_is_Start_Fire && time_count > 0)
            {
                if (time_count == time)
                {
                    _weaponSystem.ToggleFire(_spawnBullet);
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

        private void LookAtEnemy()
        {
            IPawn closestEnemy = GM.grid.FindClosestEnemy(_playerSystem);
            print("Enemy Around is found : " + (closestEnemy != null));
            if (closestEnemy != null)
            {
                Quaternion lookAt = Quaternion.LookRotation(closestEnemy.GetPawnTransform().position - transform.position);
                _coroutinesFires.Add(StartCoroutine(RotateCharacter(transform.eulerAngles, lookAt.eulerAngles, 0.5f)));
            }
        }

        IEnumerator RotateCharacter(Vector3 start, Vector3 lookat , float time)
        {
            var instruction = new WaitForEndOfFrame();
            
            float time_count = time;
            while (time_count / time > 0)
            {
                time_count -= Time.deltaTime;
                print("Time : " + (time_count / time));
                //transform.eulerAngles = Vector3.Lerp(start, new Vector3(0, lookat.y, 0), time_count / time) ;

                if (time_count <= 0)
                {
                    print("End Time!!!!!!!!!!!!!!!!!!!");
                    yield break;
                }

                yield return instruction;
            }
        }
    }
}