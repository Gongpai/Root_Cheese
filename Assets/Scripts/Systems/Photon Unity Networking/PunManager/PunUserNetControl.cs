﻿using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GDD.PUN
{
    public class PunUserNetControl : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
    {
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        [SerializeField]private Transform CameraRoot;
        public static GameObject LocalPlayerInstance;
        private GameManager GM;
        private PlayerCameraFollow _followCam;
        
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            //Game Manager
            GM = GameManager.Instance;
            
            Debug.Log(info.photonView.Owner.ToString());
            Debug.Log(info.photonView.ViewID.ToString());
            // #Important
            // used in PunNetworkManager.cs
            // : we keep track of the localPlayer instance to prevent instanciation
            // when levels are synchronized
            if (photonView.IsMine)
            {
                LocalPlayerInstance = gameObject;
                transform.parent = GM.player_layer;
                //GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
                // Reference Camera on run-time
                GameObject followCamObject = new GameObject("FollowCamera");
               _followCam = followCamObject.AddComponent<PlayerCameraFollow>();
               _followCam.root_follow = CameraRoot;
                PunNetworkManager.Instance.vCAm.Follow = _followCam.transform;
                PunNetworkManager.Instance.vCAm.LookAt = _followCam.transform;
                
                // Reference Input on run-time
                PlayerInput _pInput = GetComponent<PlayerInput>();
                _pInput.actions = PunNetworkManager.Instance.inputActionAsset;
            }
            else
            {
                GetComponent<PlayerCharacterController>().enabled = false;
                OnPlayerPropertiesUpdate(photonView.Owner, photonView.Owner.CustomProperties);

            }
        }
        
    }
}