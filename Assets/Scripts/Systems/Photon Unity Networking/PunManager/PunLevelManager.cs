﻿using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Cinemachine;
using GDD.Sinagleton;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace GDD.PUN
{
    public class PunLevelManager : CanDestroy_Sinagleton<PunLevelManager>
    {
        [Header("Level Setting")]
        [SerializeField] private GameObject m_GamePlayerPrefab;
        [SerializedDictionary("AIPrefab", "Position")]
        [SerializeField] private SerializedDictionary<GameObject, List<Transform>> m_GameAIPrefab;
        [SerializeField] private Transform m_playerLevel;
        [SerializeField] private List<Transform> m_playerSpawnPoint;
        [SerializeField] private Transform m_enemyLevel;
        [SerializeField] private string m_openLevel;
        [SerializeField] private bool _isOpenChapterLevel;
        [SerializeField] private CinemachineVirtualCamera _vCam;
        [SerializeField] private bool _isUnLoadSceneReSetGameInstance;
        [SerializeField] private bool _isReJoinLobbyOrRoom = true;

        [Header("Menu UI")] 
        [SerializeField] private GameObject m_menuUI;
        
        private GameManager GM;
        private UnityAction<bool> _sceneLoaded;

        public List<Transform> playerSpawnPoint
        {
            get => m_playerSpawnPoint;
        }

        public bool isUnLoadSceneReSetGameInstance
        {
            get => _isUnLoadSceneReSetGameInstance;
        }
        
        public GameObject GamePlayerPrefab
        {
            get => m_GamePlayerPrefab;
        }

        public SerializedDictionary<GameObject, List<Transform>>  GameAIPrefab
        {
            get
            {
                if (GM.gameState != GameState.Win || GM.gameState == GameState.GameOver)
                    return m_GameAIPrefab;
                else
                    return new SerializedDictionary<GameObject, List<Transform>>();
            } 
        }

        public string openLevel
        {
            get
            {
                if (_isOpenChapterLevel)
                    return $"CH {GM.selectChapter + 1}-{m_openLevel}";
                else
                    return m_openLevel;
            }
        }
        
        public Transform playerLevel
        {
            get => m_playerLevel;
        }
        public Transform enemyLevel
        {
            get => m_enemyLevel;
        }

        public bool isReJoinLobbyOrRoom
        {
            get => _isReJoinLobbyOrRoom;
        }

        public CinemachineVirtualCamera vCam
        {
            get => _vCam;
        }
        
        public UnityAction<bool> sceneLoaded
        {
            get => _sceneLoaded;
            set => _sceneLoaded = value;
        }

        public override void OnAwake()
        {
            base.OnAwake();
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            GM = GameManager.Instance;
            GM.player_layer = m_playerLevel;
            GM.enemy_layer = m_enemyLevel;
            GM.isUnLoadSceneReSetGameInstance = isUnLoadSceneReSetGameInstance;
            GM.openLevelName = openLevel;
            print($"Open : {GM.openLevelName}");
            GM.pauseMenuUI = m_menuUI;
            
            _sceneLoaded?.Invoke(isReJoinLobbyOrRoom);
            
            if(PhotonNetwork.IsMasterClient)
                PunNetworkManager.Instance.currentGameState = PunGameState.GamePlay;
        }

        public void UpdateLevelName()
        {
            GM.openLevelName = openLevel;
        }

        void OnSceneUnloaded(Scene scene)
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
/*
        private void OnGUI()
        {
            if(GUI.Button(new Rect(20,20,250,50), "Joint Lobby"))
                PhotonNetwork.JoinLobby();
            
            if(GUI.Button(new Rect(20,80,250,50), "Joint Lobby"))
                PhotonNetwork.JoinRoom(PunGameSetting.roomName);
        }*/

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}