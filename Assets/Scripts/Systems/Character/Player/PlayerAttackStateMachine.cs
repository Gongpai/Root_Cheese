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
        private List<Coroutine> _coroutines = new List<Coroutine>();

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

            _is_Start_Fire = true;
            
            //Reset Coriutines
            foreach (var coroutine in _coroutines)
            {
                StopCoroutine(coroutine);
            }
            _coroutines = new List<Coroutine>();
            
            //Start Coroutines Here
            IPawn closestEnemy = GM.grid.FindClosestEnemy(_playerSystem);
            LookAtEnemy(closestEnemy);
            OnFire(closestEnemy);
        }

        public override void Handle(PlayerSystem contrller)
        {
            base.Handle(contrller);
            
            IPawn closestEnemy = GM.grid.FindClosestEnemy(_playerSystem);
            print("Target null : " + (target == null));

            if (closestEnemy == null)
            {
                target = null;
                
                foreach (var coroutine in _coroutines)
                {
                    StopCoroutine(coroutine);
                }

                _coroutines = new List<Coroutine>();
            }

            if (closestEnemy != null && target == null)
            {
                if(_coroutines.Count <= 0)
                    OnFire(closestEnemy);
                
                LookAtEnemy(closestEnemy);
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            foreach (var coroutine in _coroutines)
            {
                StopCoroutine(coroutine);
            }
            
            _is_Start_Fire = false;
        }

        private void OnFire(IPawn enemy)
        {
            if (enemy != null)
            {
                _coroutines.Add(StartCoroutine(Waiting(
                    () => { _coroutines.Add(StartCoroutine(Firing(_weaponSystem.Get_Weapon.rate))); },
                    _playerSystem.delay_attack)));
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
                    //print("End Time!!!!!!!!!!!!!!!!!!!");
                    if (_isEnterState)
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

        private void LookAtEnemy(IPawn enemy)
        {
            print("Enemy Around is found : " + (enemy != null));
            if (enemy != null)
            {
                target = enemy.GetPawnTransform();
                Quaternion lookAt = Quaternion.LookRotation(enemy.GetPawnTransform().position - transform.position);
                _coroutines.Add(StartCoroutine(RotateCharacter(transform.rotation, new Quaternion(0, lookAt.y, 0, lookAt.w), 0.5f)));
            }
        }

        IEnumerator RotateCharacter(Quaternion start, Quaternion lookat , float time)
        {
            var instruction = new WaitForEndOfFrame();
            
            float time_count = 0;
            while (time_count < time)
            {
                time_count += Time.deltaTime;
                
                if (time_count >= time)
                    time_count = time;
                
                print("Time : " + (time_count / time));
                transform.rotation = Quaternion.Lerp(start, lookat, time_count / time) ;

                /*
                if (time_count >= time)
                {
                    print("End Time!!!!!!!!!!!!!!!!!!!");
                    yield break;
                }*/

                yield return instruction;
            }
        }
    }
}