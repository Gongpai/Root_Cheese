using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Cinemachine;
using ExitGames.Client.Photon;
using GDD.DataBase;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace GDD.PUN
{
    public class PunNetworkManager : MonoBehaviourPunCallbacks
    {
        //Field
        [Header("Player Spawn Settings")] [Tooltip("The prefab to use for representing the player")] 
        [SerializeField] private CinemachineVirtualCamera _vCam;
        [SerializeField] private InputActionAsset _inputActionAsset;
        [SerializeField] private GameObject GamePlayerPrefab;
        
        [Header("AI Spawn Settings")]
        [SerializeField] private GameObject GameAIPrefab;
        [SerializeField] private List<Transform> m_AISpawnTransform = new List<Transform>();
        private PunGameState _currentGameState = PunGameState.GameStart;

        [Header("Status UI")] 
        [SerializeField] private GameObject m_characterStatusUI;

        private GameObject _characterStatusUI;
        private Canvas_Element_List _canvasElementList;
        private bool _isLoadLevel;
        DataBaseController _dataBaseController;
        
        public bool isLoadLevel
        {
            get => _isLoadLevel;
            set => _isLoadLevel = value;
        }

        public GameObject characterStatusUI
        {
            get => _characterStatusUI;
        }

        public CinemachineVirtualCamera vCam
        {
            get => _vCam;
            set => _vCam = value;
        }
        
        /// <summary>
        /// Create delegate Method
        /// </summary>
        public delegate void GameStartCallback();
        public static event GameStartCallback OnGameStart;
        public delegate void GameOverCallback();
        public static event GameOverCallback OnGameOver;
        public delegate void JoinLevelCallback();
        public static event JoinLevelCallback OnJoinLevel;
        private UnityAction _onJoinLobbyAction;
        private UnityAction _onJoinConnectToMasterAction;
        private UnityAction<List<RoomInfo>> _onRoomListUpdate;
        private UnityAction _onJoinRoomAction;
        private UnityAction<short, string> _onJoinRoomFailedAction;
        private UnityAction<Room> _onPlayerEnteredRoomAction;

        public UnityAction OnJoinLobbyAction
        {
            get => _onJoinLobbyAction;
            set => _onJoinLobbyAction = value;
        }

        public UnityAction OnJoinConnectToMasterAction
        {
            get => _onJoinConnectToMasterAction;
            set => _onJoinConnectToMasterAction = value;
        }

        public UnityAction<List<RoomInfo>> OnRoomListUpdateAction
        {
            get => _onRoomListUpdate;
            set => _onRoomListUpdate = value;
        }
        
        public UnityAction OnJoinRoomAction
        {
            get => _onJoinRoomAction;
            set => _onJoinRoomAction = value;
        }
        public UnityAction<Room> OnPlayerEnteredRoomAction
        {
            get => _onPlayerEnteredRoomAction;
            set => _onPlayerEnteredRoomAction = value;
        }
        public UnityAction<short, string> OnJoinRoomFailedAction
        {
            get => _onJoinRoomFailedAction;
            set => _onJoinRoomFailedAction = value;
        }
        
        //Singleton
        protected static PunNetworkManager _instance;

        public static PunNetworkManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<PunNetworkManager>();

                return _instance;
            }
        }
        
        //Properties
        public PunGameState currentGameState
        {
            get => _currentGameState;
            set
            {
                _currentGameState = value;
                
                if (PhotonNetwork.CurrentRoom == null)
                    return;
                
                Hashtable prop = new Hashtable()
                {
                    { PunGameSetting.GAMESTATE, _currentGameState }
                };
                PhotonNetwork.CurrentRoom.SetCustomProperties(prop);
                /*
                if (PhotonNetwork.CurrentRoom.CustomProperties == null)
                {
                    Hashtable prop = new Hashtable()
                    {
                        { PunGameSetting.GAMESTATE, _currentGameState }
                    };
                    PhotonNetwork.CurrentRoom.SetCustomProperties(prop);
                }
                else
                {
                    PhotonNetwork.CurrentRoom.CustomProperties.Add(PunGameSetting.GAMESTATE, _currentGameState);
                }
                */
            }
        }
        public CinemachineVirtualCamera vCAm
        {
            get => _vCam;
        }

        public InputActionAsset inputActionAsset
        {
            get => _inputActionAsset;
        }

        private void Awake()
        {
            //Dont Destroy
            DontDestroyOnLoad(gameObject);
            
            //Scene Level
            SceneManager.sceneLoaded += OnLevelLoad;
            
            //Add Reference Method to Delegate Method
            OnGameStart += GameStartSetting;
            OnGameOver += GameOverSetting;

            PhotonNetwork.ConnectUsingSettings();
            _dataBaseController = DataBaseController.Instance;
            UpdateInfo();
            //OnCreateCharacterUI();
        }
        
        private void Update()
        {
            if(_characterStatusUI != null)
                _canvasElementList.texts[0].enabled = !PhotonNetwork.InRoom;
            
            if (!PhotonNetwork.IsMasterClient)
                return;

            switch (_currentGameState)
            {
                case PunGameState.GameStart:
                    OnGameStart();
                    break;

                case PunGameState.GamePlay:
                    //Game Loop Logic
                
                    break;
                case PunGameState.GameOver:
                    OnGameOver();
                    break;
            }
        }

        private async void UpdateInfo()
        {
            await _dataBaseController.OnSync();
            PhotonNetwork.LocalPlayer.NickName = GameManager.Instance.playerInfo.playerName;
        }
        
        private void GameStartSetting() {
            currentGameState = PunGameState.GamePlay;
        }
        
        private void GameOverSetting() {
            currentGameState = PunGameState.GameOver;
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);
            _onRoomListUpdate?.Invoke(roomList);
            
            print($"Update Room List");
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            _onPlayerEnteredRoomAction?.Invoke(PhotonNetwork.CurrentRoom);
            Debug.Log("New Player. " + newPlayer.ToString());
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            _onJoinConnectToMasterAction?.Invoke();
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            _onJoinLobbyAction?.Invoke();
            OnJoinLevel?.Invoke();
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            
            _onJoinRoomAction?.Invoke();
            
            /*
            if (PunUserNetControl.LocalPlayerInstance == null)
            {
                Debug.Log("We are Instantiating LocalPlayer from " + SceneManagerHelper.ActiveSceneName);
                PunNetworkManager.Instance.SpawnPlayer();
            }
            else
            {
                Debug.Log("Ignoring scene load for " + SceneManagerHelper.ActiveSceneName);
            }*/
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);
            _onJoinRoomFailedAction?.Invoke(returnCode, message);
            
        }

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
        }

        public void SpawnPlayer()
        {
            // we're in a room. spawn a character for the local player.
            // it gets synced by using PhotonNetwork.Instantiate
            PhotonNetwork.Instantiate(GamePlayerPrefab.name, new Vector3(5f, 5f, 2f), Quaternion.identity, 0);
            PhotonNetwork.InstantiateRoomObject(GameAIPrefab.name, new Vector3(7f, 0.1f, 5f), Quaternion.identity, 0);
        }

        private void OnCreateCharacterUI()
        {
            _characterStatusUI = Instantiate(m_characterStatusUI);
            _characterStatusUI.transform.position = Vector3.zero;
            _canvasElementList = _characterStatusUI.GetComponent<Canvas_Element_List>();
        }

        private void OnLevelLoad(Scene scene, LoadSceneMode loadSceneMode)
        {
            PunLevelManager PLM = PunLevelManager.Instance;
            PLM.sceneLoaded = () =>
            {
                if(_characterStatusUI == null)
                    OnCreateCharacterUI();
                
                if (PhotonNetwork.InRoom && _isLoadLevel && PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.DestroyAll();
                    GameManager.Instance.players = new SerializedDictionary<PlayerSystem, bool>();
                    _currentGameState = PunGameState.GameOver;
                    PunNetworkManager.Instance.SpawnPlayer();
                }else if (PhotonNetwork.InRoom && !PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.LeaveRoom();

                    if (PunGameSetting.roomName != "")
                    {
                        OnJoinLevel = () =>
                        {
                            PhotonNetwork.JoinRoom(PunGameSetting.roomName);

                            OnJoinLevel -= OnJoinLevel;
                        };
                    }

                    GameManager.Instance.players = new SerializedDictionary<PlayerSystem, bool>();
                    PunNetworkManager.Instance.SpawnPlayer();
                }
            };
        }
        
        public void gameStateUpdate(Hashtable propertiesThatChanged) {
            object gameStateFromProps;

            if (propertiesThatChanged.TryGetValue(PunGameSetting.GAMESTATE, out gameStateFromProps)) {
                Debug.Log("GetStartTime Prop is : " + (PunGameState)gameStateFromProps);
                _currentGameState = (PunGameState)gameStateFromProps;
            }

            if(_currentGameState == PunGameState.GameOver)
                OnGameOver();
        }
        
        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {
            base.OnRoomPropertiesUpdate(propertiesThatChanged);
            gameStateUpdate(propertiesThatChanged);
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }
    }
}