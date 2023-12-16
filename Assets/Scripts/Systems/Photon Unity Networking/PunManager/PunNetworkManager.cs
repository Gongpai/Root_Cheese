using System;
using Cinemachine;
using ExitGames.Client.Photon;
using GDD.Sinagleton;
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
        [Header("Player Spawn Settings")][Tooltip("The prefab to use for representing the player")]
        [SerializeField] private PlayerCameraFollow _followCam;
        [SerializeField] private InputActionAsset _inputActionAsset;
        [SerializeField] private Transform _playerParent;
        [SerializeField] private GameObject GamePlayerPrefab;
        private PunGameState _currentGameState = PunGameState.GameStart;
        
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
                    { PunGameSetting.GAMESTATE, _currentGameState.ToString() }
                };
                PhotonNetwork.CurrentRoom.SetCustomProperties(prop);
            }
        }
        public PlayerCameraFollow followCam
        {
            get => _followCam;
        }

        public InputActionAsset inputActionAsset
        {
            get => _inputActionAsset;
        }

        private void Awake()
        {
            //Add Reference Method to Delegate Method
            OnGameStart += GameStartSetting;
        }

        private void Update()
        {
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
            }
        }

        private void GameStartSetting() {
            currentGameState = PunGameState.GamePlay;
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
            GameObject player = PhotonNetwork.Instantiate(GamePlayerPrefab.name, new Vector3(5f, 2f, 2f), Quaternion.identity, 0);
            print("Player Name :: " + player.name);
            player.transform.parent = _playerParent;
        }
        
        public void gameStateUpdate(Hashtable propertiesThatChanged) {
            object gameStateFromProps;

            if (propertiesThatChanged.TryGetValue(PunGameSetting.GAMESTATE, out gameStateFromProps)) {
                Debug.Log("GetStartTime Prop is : " + gameStateFromProps);
                _currentGameState = (PunGameState)Enum.Parse(typeof(PunGameState), (string)gameStateFromProps);
            }

            if(_currentGameState == PunGameState.GameOver)
                OnGameOver();
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {
            base.OnRoomPropertiesUpdate(propertiesThatChanged);
            gameStateUpdate(propertiesThatChanged);
        }
    }
}