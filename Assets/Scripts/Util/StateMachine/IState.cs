using UnityEditor;
using UnityEngine;

namespace GDD.StateMachine
{
    public interface IState<T>
    {
        public string StateName();
        
        public void OnStart(T contrller);
        
        public void Handle(T contrller);

        public void OnExit();
    }
}