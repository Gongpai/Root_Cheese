using GDD.StateMachine;
using Unity.VisualScripting;
using UnityEngine;

namespace GDD
{
    public class ProjectileJumpState : MonoBehaviour, IAiState
    {
        protected void Start()
        {
            
        }
        
        public Vector3 EnterState()
        {
            return new Vector3();
        }

        public void UpdateState()
        {
            
        }

        public void OnExitState()
        {
            CustomEvent.Trigger(gameObject,"GotoIdleState");
        }

        public void ExitState()
        {
            
        }
    }
}