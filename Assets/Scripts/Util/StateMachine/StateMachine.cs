using System;
using Unity.VisualScripting;
using UnityEngine;

namespace GDD.StateMachine
{
    public abstract class StateMachine<T> : MonoBehaviour, IState<T>
    {
        protected bool _isEnterState = false;
        private T current_contrller;

        protected virtual void Start()
        {
            
        }

        public abstract string StateName();

        public virtual void OnStart(T contrller)
        {
            
        }

        public virtual void Handle(T contrller)
        {
            //print("Enter Handle");
            current_contrller = contrller;
            
            if(!_isEnterState)
                OnStart(current_contrller);
            
            _isEnterState = true;
        }

        public virtual void OnExit()
        {
            _isEnterState = false;
            //print("OnExitState : " + StateName());
        }
    }
}