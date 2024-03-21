using System;
using GDD.PUN;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GDD
{
    public class PauseMenuUI : UI
    {
        [SerializeField] private string m_nameMainMenu;
        [SerializeField] private Button m_RejointButton;
        protected GameManager GM;
        protected PunLevelManager PLM;

        protected virtual void Start()
        {
            GM = GameManager.Instance;
            PLM = PunLevelManager.Instance;
        }

        private void Update()
        {
            if (m_RejointButton != null)
                m_RejointButton.interactable = GM.players.Count > 1;
            
            if(Input.GetKeyDown(KeyCode.Escape))
                gameObject.SetActive(false);
        }

        public async void OnBackToMainMenu()
        {
            SceneManager.sceneUnloaded += ResetGrid;
            await GM.PunLoadLevel(m_nameMainMenu);
            PhotonNetwork.LeaveRoom();
        }

        private void ResetGrid(Scene scene)
        {
            GM.ResetGird();
            SceneManager.sceneUnloaded -= ResetGrid;
        }
        
        public void OnReOpenLevelScene()
        {
            PhotonNetwork.LeaveRoom();
            PunNetworkManager.Instance.OnJoinConnectToMasterAction += OnLeaveRoomToLobby;
        }

        private void OnLeaveRoomToLobby()
        {
            PhotonNetwork.JoinRoom(PunGameSetting.roomName);
            PunNetworkManager.Instance.OnJoinConnectToMasterAction -= OnLeaveRoomToLobby;
        }

        private void OnDisable()
        {
            GameStateManager.Instance.SetState(GameState.Playing);
        }
    }
}