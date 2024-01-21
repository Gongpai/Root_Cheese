using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GDD.PUN
{
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(PhotonTransformView))]
    public class PunUserNetControl : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
    {
        [Header("Camera")]
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        [SerializeField]private Transform CameraRoot;

        [Header("Status UI")] 
        [SerializeField] private GameObject m_statusUI;
        
        public static GameObject LocalPlayerInstance;
        protected GameManager GM;
        private PlayerCameraFollow _followCam;
        private GameObject _statusUI;
        
        public virtual void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            //Game Manager
            GM = GameManager.Instance;
            
            transform.parent = GM.player_layer;
            GetComponent<PlayerSystem>().idPhotonView = photonView.ViewID;
            
            //Add Status Character UI
            AddStatusUI(GetComponent<CharacterSystem>());
            
            //Debug.Log(info.photonView.Owner.ToString());
            //Debug.Log(info.photonView.ViewID.ToString());
            // #Important
            // used in PunNetworkManager.cs
            // : we keep track of the localPlayer instance to prevent instanciation
            // when levels are synchronized
            if (photonView.IsMine)
            {
                gameObject.name += "[MasterClient]";
                LocalPlayerInstance = gameObject;
                //GM.players.Add(GetComponent<PlayerSystem>());
                //GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
                // Reference Camera on run-time
                GameObject followCamObject = new GameObject("FollowCamera");
               _followCam = followCamObject.AddComponent<PlayerCameraFollow>();
               _followCam.root_follow = CameraRoot;
                PunNetworkManager.Instance.vCAm.Follow = _followCam.transform;
                PunNetworkManager.Instance.vCAm.LookAt = _followCam.transform;
                
                //Set Player Host
                GM.playerMasterClient = LocalPlayerInstance.GetComponent<PlayerSystem>();
                
                // Reference Input on run-time
                PlayerInput _pInput = GetComponent<PlayerInput>();
                _pInput.actions = PunNetworkManager.Instance.inputActionAsset;
            }
            else
            {
                gameObject.name += $" [Other] [{photonView.ViewID}]";
                //GM.players.Add(GetComponent<PlayerSystem>());
                GetComponent<CharacterControllerSystem>().enabled = false;
                GetComponent<PlayerSystem>().isMasterClient = false;
                OnPlayerPropertiesUpdate(photonView.Owner, photonView.Owner.CustomProperties);
                
            }
        }

        public void AddStatusUI(CharacterSystem characterSystem)
        {
            PunNetworkManager PNM = PunNetworkManager.Instance;
            _statusUI = Instantiate(m_statusUI, PNM.characterStatusUI.GetComponent<Canvas_Element_List>().canvas_gameObjects[0].transform);
            _statusUI.transform.localPosition = Vector3.zero;
            _statusUI.GetComponent<CharacterStatusUI>().characterSystem = characterSystem;
        }

        public void OnDestroy()
        {
            RemoveStatusUI();
        }

        public void RemoveStatusUI()
        {
            Destroy(_statusUI);
        }
        
        public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps) {
            base.OnPlayerPropertiesUpdate(target, changedProps);
        }
    }
}