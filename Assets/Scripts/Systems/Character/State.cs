using System;
using Unity.VisualScripting;
using UnityEngine;

namespace GDD
{
    public abstract class State<T> : MonoBehaviour, IState<T>
    {
        protected PlayerSystem _playerSystem;
        protected bool _isEnterState = false;
        private T current_contrller;

        protected virtual void Start()
        {
            _playerSystem = GetComponent<PlayerSystem>();
        }

        public abstract string StateName();

        public virtual void OnStart(T contrller)
        {
            print("OnStartState : " + StateName());
        }

        public virtual void Handle(T contrller)
        {
            current_contrller = contrller;
            
            if(!_isEnterState)
                OnStart(current_contrller);
            
            _isEnterState = true;
        }

        public virtual void OnExit()
        {
            _isEnterState = false;
            print("OnExitState : " + StateName());
        }
    }
}