using System;
using ExitGames.Client.Photon;
using GDD.Util;
using Newtonsoft.Json;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace GDD.PUN
{
    public class PunPuppetAIWaypointReachingState : MonoBehaviourPun
    {
        [SerializeField] protected MultiplayerEnemyController m_multiplayerEnemyController;
        [SerializeField] protected NavMeshAgent m_NavMeshAgent;
        private WaypointReachingState _waypointReachingState;
        private bool isEnterState = true;
        private bool ismove;
        
        private void Start()
        {
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            m_multiplayerEnemyController = GetComponent<MultiplayerEnemyController>();
        }
        
        public void EnterStateEvent(Vector3 position)
        {
            print("Call Raise Event : Ai Enter State");
            
            object[] content;
            content = new object[]
            {
                photonView.ViewID,
                JsonConvert.SerializeObject(new float3D(position.x, position.y, position.z)) 
            };

            photonView.RPC("PunEnterState", RpcTarget.Others, content);
        }
        
        public void ExitStateEvent()
        {
            print("Call Raise Event : Ai Exit State");
            
            photonView.RPC("PunExitState", RpcTarget.Others, photonView.ViewID);
        }

        private void Update()
        {
            if(!isEnterState && photonView.IsMine)
                return;
            
            UpdateState();
        }
        
        public void UpdateState()
        {
            if (m_NavMeshAgent.remainingDistance > m_NavMeshAgent.stoppingDistance)
            {
                if (!ismove)
                {
                    ismove = true;
                    print("Moveeeeeeeeeee");
                }

                m_multiplayerEnemyController.Move(m_NavMeshAgent.desiredVelocity);
            }
            else
            {
                if(ismove)
                    print("Stoppppppppppppp");
                
                ismove = false;
                isEnterState = false;
            }
        }

        [PunRPC]
        public void PunEnterState(object[] data)
        {
            if (!photonView.IsMine)
            {
                float3D pos = JsonConvert.DeserializeObject<float3D>((string)data[1]);
                print($"Ai Enter State {pos}");

                if ((int)data[0] == photonView.ViewID)
                {
                    m_NavMeshAgent.SetDestination(new Vector3(pos.x, pos.y, pos.z));
                    isEnterState = true;
                }
            }
        }
        
        [PunRPC]
        public void PunExitState(int viewID)
        {
            if (!photonView.IsMine)
            {
                m_multiplayerEnemyController.Move(Vector3.zero);
                isEnterState = false;
                
                if(ismove)
                    print("Exit Stoppppppppppppp");
                
                ismove = false;
            }
        }
    }
}