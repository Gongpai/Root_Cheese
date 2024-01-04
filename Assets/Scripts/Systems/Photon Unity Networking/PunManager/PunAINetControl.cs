using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace GDD.PUN
{
    public class PunAINetControl : MonoBehaviourPun, IPunInstantiateMagicCallback
    {
        [Header("Enemy Host Component")] 
        [SerializeField]
        private List<Behaviour> m_hostComponents = new List<Behaviour>();

        private GameManager GM;
        
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            //Game Manager
            GM = GameManager.Instance;
            transform.parent = GM.enemy_layer;
            
            Debug.Log(info.photonView.Owner.ToString());
            Debug.Log(info.photonView.ViewID.ToString());
            // #Important
            // used in PunNetworkManager.cs
            // : we keep track of the localPlayer instance to prevent instanciation
            // when levels are synchronized
            if (photonView.IsMine)
            {
                
            }
            else
            {
                gameObject.name += $" [Other Player] [{photonView.ViewID}]";
                
                foreach (var component in m_hostComponents)
                {
                    component.enabled = false;
                }
            }
        }
    }
}