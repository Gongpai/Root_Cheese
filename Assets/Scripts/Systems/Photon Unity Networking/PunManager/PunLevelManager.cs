using System;
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
        [SerializeField] private Transform m_playerLevel;
        [SerializeField] private Transform m_enemyLevel;
        [SerializeField] private CinemachineVirtualCamera _vCam;
        private GameManager GM;
        private UnityAction _sceneLoaded;

        public UnityAction sceneLoaded
        {
            get => _sceneLoaded;
            set => _sceneLoaded = value;
        }

        public override void OnAwake()
        {
            base.OnAwake();
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            GM = GameManager.Instance;
            GM.player_layer = m_playerLevel;
            GM.enemy_layer = m_enemyLevel;

            PunNetworkManager.Instance.vCam = _vCam;
            
            _sceneLoaded?.Invoke();
            
            if(PhotonNetwork.IsMasterClient)
                PunNetworkManager.Instance.currentGameState = PunGameState.GamePlay;
        }
    }
}