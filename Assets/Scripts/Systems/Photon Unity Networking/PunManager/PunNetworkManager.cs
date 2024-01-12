using System;
using System.Collections.Generic;
using Cinemachine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GDD.PUN
{
    public class PunNetworkManager : ConnectAndJoinRandom
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

        public GameObject characterStatusUI
        {
            get => _characterStatusUI;
        }
        
        /// <summary>
        /// Create delegate Method
        /// </summary>
        public delegate void GameStartCallback();
        public static event GameStartCallback OnGameStart;
        public delegate void GameOverCallback();
        public static event GameOverCallback OnGameOver;
        
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
            //Add Reference Method to Delegate Method
            OnGameStart += GameStartSetting;
            OnGameOver += GameOverSetting;
            
            //Create Character Status UI
            _characterStatusUI = Instantiate(m_characterStatusUI);
            _characterStatusUI.transform.position = Vector3.zero;
            _canvasElementList = _characterStatusUI.GetComponent<Canvas_Element_List>();
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

        private void GameStartSetting() {
            currentGameState = PunGameState.GamePlay;
        }
        
        private void GameOverSetting() {
            currentGameState = PunGameState.GameOver;
        }
        
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            Debug.Log("New Player. " + newPlayer.ToString());
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
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

        public void SpawnPlayer()
        {
            // we're in a room. spawn a character for the local player.
            // it gets synced by using PhotonNetwork.Instantiate
            PhotonNetwork.Instantiate(GamePlayerPrefab.name, new Vector3(5f, 5f, 2f), Quaternion.identity, 0);
            PhotonNetwork.InstantiateRoomObject(GameAIPrefab.name, new Vector3(7f, 0.1f, 5f), Quaternion.identity, 0);
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