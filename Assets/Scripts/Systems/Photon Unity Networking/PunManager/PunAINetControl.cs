using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace GDD.PUN
{
    public class PunAINetControl : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
    {
        [Header("Enemy Host Component")] 
        [SerializeField]
        private List<Behaviour> m_hostComponents = new List<Behaviour>();
        private bool isMine;
        private string gameObjectName;

        private GameManager GM;
        
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            //Game Manager
            GM = GameManager.Instance;
            
            transform.parent = GM.enemy_layer;
            isMine = photonView.IsMine;
            gameObjectName = gameObject.name;
            GetComponent<EnemySystem>().idPhotonView = photonView.ViewID;
            
            Debug.Log(info.photonView.Owner.ToString());
            Debug.Log(info.photonView.ViewID.ToString());
            // #Important
            // used in PunNetworkManager.cs
            // : we keep track of the localPlayer instance to prevent instanciation
            // when levels are synchronized
            if (photonView.IsMine)
            {
                gameObject.name = $"{gameObjectName} [MasterClient]";
            }
            else
                AIIsNotMine();
        }

        private void Update()
        {
            if (isMine != photonView.IsMine)
            {
                //print($"Is Mine : {isMine} || Photon Is Mine : {photonView.IsMine}");
                
                //Logic
                if(photonView.IsMine)
                    AIIsMine();
                else
                    AIIsNotMine();
                
                isMine = photonView.IsMine;
            }
        }

        private void AIIsMine()
        {
            gameObject.name = $"{gameObjectName} [MasterClient]";
            GetComponent<EnemySystem>().isMasterClient = photonView.IsMine;
                
            foreach (var component in m_hostComponents)
            {
                component.enabled = true;
            }
        }
        
        private void AIIsNotMine()
        {
            gameObject.name = $"{gameObjectName} [Other] [{photonView.ViewID}]";
            GetComponent<EnemySystem>().isMasterClient = photonView.IsMine;
                
            foreach (var component in m_hostComponents)
            {
                component.enabled = false;
            }
        }
        
        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            
            print($"Character Left Room : {gameObject.name}");
            print($"Photon is mine : {photonView.IsMine}");
        }
    }
}