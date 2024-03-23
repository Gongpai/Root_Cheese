using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

namespace GDD
{
    public class IdleState : MonoBehaviour, IAiState
    {
        protected float m_IdleTime;
        protected BulletFireManeuver _bulletFireManeuver;

        private void Start()
        {
            _bulletFireManeuver = GetComponent<BulletFireManeuver>();
        }

        public Vector3 EnterState()
        {
            RandomIdleTime();
            
            return Vector3.zero;
        }

        private void RandomIdleTime()
        {
            if(_bulletFireManeuver == null)
                _bulletFireManeuver = GetComponent<BulletFireManeuver>();
            
            m_IdleTime = UnityEngine.Random.Range(2, 5) + _bulletFireManeuver.timeDelay;
        }
        
        public void UpdateState()
        {
            ReduceIdleTimer();
        }

        private void ReduceIdleTimer()
        {
            m_IdleTime -= Time.deltaTime;
            if (m_IdleTime <= 0)
            {
                CustomEvent.Trigger(gameObject,"GotoMoveState");
            }
        }

        public void ExitState()
        {
            
        }
    }
}