using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GDD.PUN
{
    public class PunUserNetControl : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
    {
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        [SerializeField]private Transform CameraRoot;
        public static GameObject LocalPlayerInstance;
        
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            Debug.Log(info.photonView.Owner.ToString());
            Debug.Log(info.photonView.ViewID.ToString());
// #Important
// used in PunNetworkManager.cs
// : we keep track of the localPlayer instance to prevent instanciation
// when levels are synchronized
            if (photonView.IsMine)
            {
                LocalPlayerInstance = gameObject;
                GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
// Reference Camera on run-time
                PunNetworkManager.Instance.followCam.root_follow = CameraRoot;
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