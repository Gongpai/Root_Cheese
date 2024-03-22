using System;
using UnityEngine;

namespace GDD.StateMachine
{
    public class StateContext<T>
    {
        public IState<T> CurrentState { get; set; }

        private readonly T _controller;

        public StateContext(T controller)
        {
            _controller = controller;
        }

        public void Transition(IState<T> state)
        {
            if (CurrentState != null && CurrentState != state)
                CurrentState.OnExit();
            
            Debug.Log("Transition");
            CurrentState = state;
            CurrentState.Handle(_controller);
            
            Debug.Log($"Current State : {CurrentState.StateName()}");
        }
    }
}