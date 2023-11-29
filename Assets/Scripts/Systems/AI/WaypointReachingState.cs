using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace GDD
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonControllerAI))]
    public class WaypointReachingState : MonoBehaviour
    {
        [SerializeField] protected int m_CurrentWaypointIndex = 0;

        [SerializeField] protected ThirdPersonControllerAI m_3rdPersonControllerAI;
       
        [SerializeField] protected NavMeshAgent m_NavMeshAgent;

        [SerializeField] protected List<Transform> m_Waypoints;

        public List<Transform> _waypoints
        {
            get => m_Waypoints;
            set => m_Waypoints = value;
        }
        
        private void Start()
        {
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            m_3rdPersonControllerAI = GetComponent<ThirdPersonControllerAI>();
        }

        public void EnterState()
        {
            m_NavMeshAgent.SetDestination(m_Waypoints[m_CurrentWaypointIndex].position);
        }
        
        public void UpdateState()
        {
            if (m_NavMeshAgent.remainingDistance > m_NavMeshAgent.stoppingDistance)
            {
                //print("Moveeeeeeeeeee");
                m_3rdPersonControllerAI.Move(m_NavMeshAgent.desiredVelocity);
            }
            else
            {
                m_3rdPersonControllerAI.Move(Vector3.zero);

                if (m_CurrentWaypointIndex + 1 < m_Waypoints.Count)
                {
                    m_CurrentWaypointIndex++;
                    CustomEvent.Trigger(gameObject,"GotoIdleState");
                }
                else
                {//reach the maximum of the available waypoints, then go back to 0
                    m_CurrentWaypointIndex = 0;
                    CustomEvent.Trigger(gameObject,"GotoIdleState");
                }
            }
        }

        public void ExitState()
        {
            
        }

    }
}