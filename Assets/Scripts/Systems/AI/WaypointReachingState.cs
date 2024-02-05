using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace GDD
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(MultiplayerEnemyController))]
    public class WaypointReachingState : MonoBehaviour
    {
        [SerializeField] protected int m_CurrentWaypointIndex = 0;

        [SerializeField] protected MultiplayerEnemyController m_multiplayerEnemyController;
       
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
            m_multiplayerEnemyController = GetComponent<MultiplayerEnemyController>();
        }

        public Vector3 EnterState()
        {
            Vector3 pos;
            pos = m_Waypoints[m_CurrentWaypointIndex].position;
            m_NavMeshAgent.SetDestination(pos);

            return pos;
        }

        public void UpdateState()
        {
            if (m_NavMeshAgent.remainingDistance > m_NavMeshAgent.stoppingDistance)
            {
                //print("Moveeeeeeeeeee");
                m_multiplayerEnemyController.Move(m_NavMeshAgent.desiredVelocity);
            }
            else
            {
                m_multiplayerEnemyController.Move(Vector3.zero);

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