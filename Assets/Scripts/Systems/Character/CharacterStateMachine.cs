using System;
using System.Collections.Generic;
using GDD.Spatial_Partition;
using GDD.StateMachine;
using UnityEngine;

namespace GDD
{
    public class CharacterStateMachine<T> : StateMachine<T>
    {
        protected T _characterSystem;
        protected GameManager GM;
        protected Transform target;
        protected PlayerSpawnBullet PlayerSpawnBullet;
        protected bool _is_Start_Fire = false;
        protected List<Coroutine> _coroutines = new List<Coroutine>();
        
        protected virtual void Start()
        {
            GM = GameManager.Instance;
            _characterSystem = GetComponent<T>();
        }

        protected virtual void Update()
        {
            
        }

        protected void ClearCoriutines()
        {
            foreach (var coroutine in _coroutines)
            {
                if(coroutine != null)
                    StopCoroutine(coroutine);
            }
            _coroutines = new List<Coroutine>();
        }
        
        public override string StateName()
        {
            return "State";
        }
        
        public override void OnStart(T contrller)
        {
            base.OnStart(contrller);
            
            if(GM == null)
                GM = GameManager.Instance;
            
            if(_characterSystem == null)
                _characterSystem = GetComponent<T>();
            
            //Clear All Coriutines
            ClearCoriutines();
        }

        public override void Handle(T contrller)
        {
            base.Handle(contrller);
        }

        public override void OnExit()
        {
            base.OnExit();
            
            //Clear All Coriutines
            ClearCoriutines();
        }
    }
}