using System;
using System.Collections.Generic;
using System.Linq;
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
        
        [Header("AI Spawn Settings")]
        [SerializeField] private List<Transform> m_AISpawnTransform = new List<Transform>();
        private PunGameState _currentGameState = PunGameState.GameStart;

        [Header("Status UI")] 
        [SerializeField] private GameObject m_characterStatusUI;

        private GameObject _characterStatusUI;
        private Canvas_Element_List _canvasElementList;
        private bool _isLoadLevel;
        DataBaseController _dataBaseController;
        private PunLevelManager PLM;
        
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
        public delegate void JoinLeftLevelCallback();
        public static event JoinLeftLevelCallback OnLeftLevel;
        private UnityAction _onJoinLobbyAction;
        private UnityAction _onJoinConnectToMasterAction;
        private UnityAction<List<RoomInfo>> _onRoomListUpdate;
        private UnityAction _onJoinRoomAction;
        private UnityAction<short, string> _onJoinRoomFailedAction;
        private UnityAction<Room> _onPlayerEnteredRoomAction;
        private UnityAction<List<FriendInfo>> _onFriendListUpdateAction;
        private UnityAction<List<Player>> _onPlayerListUpdateAction;
        private UnityAction _onLeftRoomAction;
        private List<RoomInfo> _currentRoomList = new List<RoomInfo>();

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
        
        public UnityAction<List<FriendInfo>> OnFriendListUpdateAction
        {
            get => _onFriendListUpdateAction;
            set => _onFriendListUpdateAction = value;
        }
            
        public UnityAction<List<Player>> OnPlayerListUpdateAction
        {
            get => _onPlayerListUpdateAction;
            set => _onPlayerListUpdateAction = value;
        }
        
        public UnityAction OnLeftRoomAction
        {
            get => _onLeftRoomAction;
            set => _onLeftRoomAction = value;
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
                
                if (!PhotonNetwork.InRoom || !PhotonNetwork.InLobby || !PhotonNetwork.IsConnected)
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
            
            //Add Reference Method to Delegate Method
            OnGameStart += GameStartSetting;
            OnGameOver += GameOverSetting;

            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.EnableCloseConnection = true;
            _dataBaseController = DataBaseController.Instance;
            UpdateInfo();
            //OnCreateCharacterUI();
        }

        public override void OnEnable()
        {
            base.OnEnable();
            
            //Scene Level
            SceneManager.sceneLoaded += OnLevelLoad;
            SceneManager.sceneUnloaded += OnLevelUnLoad;
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
            _currentRoomList = roomList;
            _onRoomListUpdate?.Invoke(roomList);
            
            print($"Update Room List");
        }

        public void OnReUpdateRoomList()
        {
            if(!PhotonNetwork.InLobby)
                return;
            
            _onRoomListUpdate?.Invoke(_currentRoomList);
        }
        
        public override void OnFriendListUpdate(List<FriendInfo> friendList)
        {
            base.OnFriendListUpdate(friendList);
            _onFriendListUpdateAction?.Invoke(friendList);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            _onPlayerEnteredRoomAction?.Invoke(PhotonNetwork.CurrentRoom);
            _onPlayerListUpdateAction?.Invoke(PhotonNetwork.CurrentRoom.Players.Values.ToList());
            Debug.Log("New Player. " + newPlayer.ToString());
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            _onPlayerListUpdateAction?.Invoke(PhotonNetwork.CurrentRoom.Players.Values.ToList());
        }

        public void OnReCheckPlayerRoom()
        {
            if(!PhotonNetwork.InRoom)
                return;
            
            var players = PhotonNetwork.CurrentRoom.Players;
            
            if(players != null)
                _onPlayerListUpdateAction?.Invoke(players.Values.ToList());
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            _onJoinConnectToMasterAction?.Invoke();
            print("OnConnectedToMaster !!!!!!!!!");
            
            OnLeftLevel?.Invoke();
            OnJoinLevel = () =>
            {
                PhotonNetwork.AutomaticallySyncScene = true;

                OnJoinLevel -= OnJoinLevel;
            };
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            _onJoinLobbyAction?.Invoke();
            OnJoinLevel?.Invoke();
            print("Join Lobby!!!!!!!!!");
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            print("Join Roommmm!!!!!!!!!");
            _onJoinRoomAction?.Invoke();
            _onPlayerListUpdateAction?.Invoke(PhotonNetwork.CurrentRoom.Players.Values.ToList());
            
            if (PunUserNetControl.LocalPlayerInstance == null)
            {
                Debug.Log("We are Instantiating LocalPlayer from " + SceneManagerHelper.ActiveSceneName);
                PunNetworkManager.Instance.SpawnPlayer();
            }
            else
            {
                Debug.Log("Ignoring scene load for " + SceneManagerHelper.ActiveSceneName);
            }
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);
            Debug.LogError(message);
            _onJoinRoomFailedAction?.Invoke(returnCode, message);
            
        }

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            print("Left Room!!!!!!");
            _onJoinRoomAction?.Invoke();
            _onLeftRoomAction?.Invoke();
            _onPlayerListUpdateAction?.Invoke(new List<Player>());
            OnReUpdateRoomList();
        }

        public override void OnLeftLobby()
        {
            base.OnLeftLobby();
            print("Left Lobby!!!!!!!!!!!!!!");
            
            _onPlayerListUpdateAction?.Invoke(new List<Player>());
        }

        public void SpawnPlayer()
        {
            // we're in a room. spawn a character for the local player.
            // it gets synced by using PhotonNetwork.Instantiate
            
            PLM = PunLevelManager.Instance;
            _vCam = PLM.vCam;
            if (PLM.GamePlayerPrefab != null)
            {
                Transform spawnPoint = PLM.playerSpawnPoint[PhotonNetwork.PlayerList.ToList().IndexOf(PhotonNetwork.LocalPlayer)];
                PhotonNetwork.Instantiate(PLM.GamePlayerPrefab.name, spawnPoint.position, spawnPoint.rotation, 0);
            }

            if (PLM.GameAIPrefab != null)
            {
                for (int i = 0; i < PLM.GameAIPrefab.Count; i++)
                {
                    GameObject objectPrefab = PLM.GameAIPrefab.Keys.ElementAt(i);
                    for (int j = 0; j < PLM.GameAIPrefab[objectPrefab].Count; j++)
                    {
                        Transform transformSpawn = PLM.GameAIPrefab[objectPrefab][j];
                        PhotonNetwork.InstantiateRoomObject(objectPrefab.name, transformSpawn.position, transformSpawn.rotation);
                    }
                }
            }
        }

        private void OnCreateCharacterUI()
        {
            _characterStatusUI = Instantiate(m_characterStatusUI);
            _characterStatusUI.transform.position = Vector3.zero;
            _canvasElementList = _characterStatusUI.GetComponent<Canvas_Element_List>();
        }

        private void OnLevelLoad(Scene scene, LoadSceneMode loadSceneMode)
        {
            PLM = PunLevelManager.Instance;
            
            PLM.sceneLoaded = isLobby =>
            {
                if(_characterStatusUI == null && isLobby)
                    OnCreateCharacterUI();

                if (PhotonNetwork.InRoom)
                {
                    if (isLobby)
                    {
                        GameManager.Instance.players = new SerializedDictionary<PlayerSystem, bool>();
                        _currentGameState = PunGameState.GameOver;
                    }

                    PunNetworkManager.Instance.SpawnPlayer();
                }
                
                /*

                if (PhotonNetwork.InRoom && _isLoadLevel && PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.DestroyAll();
                    GameManager.Instance.players = new SerializedDictionary<PlayerSystem, bool>();
                    _currentGameState = PunGameState.GameOver;
                    PunNetworkManager.Instance.SpawnPlayer();
                }
                else
                {
                    print($"2 Room in pun = {PunGameSetting.roomName}");
                    if (PunGameSetting.roomName != "")
                    {
                        print($"Joinnnnnnnnnnnn!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                        PhotonNetwork.JoinLobby();
                        OnLeftLevel = () =>
                        {
                            PhotonNetwork.JoinLobby();

                            OnLeftLevel -= OnLeftLevel;
                        };

                    }

                    GameManager.Instance.players = new SerializedDictionary<PlayerSystem, bool>();
                    PunNetworkManager.Instance.SpawnPlayer();
                }
                */
            };
        }

        public void OnLevelUnLoad(Scene scene)
        {
            
        }
        
        public void gameStateUpdate(Hashtable propertiesThatChanged) {
            object gameStateFromProps;

            if (propertiesThatChanged.TryGetValue(PunGameSetting.GAMESTATE, out gameStateFromProps)) {
                //Debug.Log("GetStartTime Prop is : " + (PunGameState)gameStateFromProps);
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
            
            //Scene Level
            SceneManager.sceneLoaded -= OnLevelLoad;
            SceneManager.sceneUnloaded -= OnLevelUnLoad;
        }
    }
}