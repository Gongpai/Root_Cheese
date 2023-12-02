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
        private WeaponSystem _weaponSystem;
        private bool _is_end_rotation = false;
        
        protected override void Start()
        {
            base.Start();
            
            PlayerSpawnBullet = GetComponent<PlayerSpawnBullet>();
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
            
            //Start Coroutines Here
            IPawn closestEnemy = GM.grid.FindClosestEnemy(_characterSystem);
            SmoothLookAtEnemy(closestEnemy);
            OnFire(closestEnemy);
        }

        public override void Handle(PlayerSystem contrller)
        {
            base.Handle(contrller);
            
            print("Character System = null : " + (_characterSystem == null));
            IPawn closestEnemy = GM.grid.FindClosestEnemy(_characterSystem);
            //print("ClosestEnemy null : " + (closestEnemy == null));

            if (closestEnemy == null)
            {
                target = null;
                
                //Reset Coriutines
                ClearCoriutines();
            }

            if (closestEnemy != null && closestEnemy.GetPawnTransform() != target)
            {
                if(_coroutines.Count <= 0)
                    OnFire(closestEnemy);
                
                SmoothLookAtEnemy(closestEnemy);
            }
            
            //Debug.Log("IS TRUE : " + _is_end_rotation);
            if(target != null && _is_end_rotation)
                LookAtEnemy(target);
                
        }

        public override void OnExit()
        {
            base.OnExit();
            
            _is_Start_Fire = false;
        }

        private void OnFire(IPawn enemy)
        {
            if (enemy != null)
            {
                _coroutines.Add(StartCoroutine(Waiting(
                    () => { _coroutines.Add(StartCoroutine(Firing(_weaponSystem.Get_Weapon.rate))); },
                    _characterSystem.delay_attack)));
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
                    _weaponSystem.ToggleFire(PlayerSpawnBullet);
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

        private void SmoothLookAtEnemy(IPawn enemy)
        {
            //print("Enemy Around is found : " + (enemy != null));
            if (enemy != null)
            {
                target = enemy.GetPawnTransform();
                Quaternion lookAt = Quaternion.LookRotation(enemy.GetPawnTransform().position - transform.position);
                _coroutines.Add(StartCoroutine(RotateCharacter(transform.rotation, new Quaternion(0, lookAt.y, 0, lookAt.w), 0.25f)));
            }
        }

        private void LookAtEnemy(Transform pawn)
        {
            Quaternion lookAt = Quaternion.LookRotation(pawn.position - transform.position);
            transform.rotation = new Quaternion(0, lookAt.y, 0, lookAt.w);
            //print("Time : ");
        }

        IEnumerator RotateCharacter(Quaternion start, Quaternion lookat , float time)
        {
            var instruction = new WaitForEndOfFrame();
            
            float time_count = 0;
            while (time_count < time)
            {
                time_count += Time.deltaTime;

                _is_end_rotation = false;
                if (time_count >= time)
                {
                    time_count = time;
                    _is_end_rotation = true;
                }
                
                transform.rotation = Quaternion.Lerp(start, lookat, time_count / time) ;

                yield return instruction;
            }
        }
    }
}