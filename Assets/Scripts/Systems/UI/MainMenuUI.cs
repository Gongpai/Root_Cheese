using System;
using GDD.DataBase;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class MainMenuUI : UI
    {
        [Header("Play Panel")] 
        [SerializeField] private Button m_playButton;
        
        [Header("Option Panel")] 
        [SerializeField] private TextMeshProUGUI m_op;

        [Header("Player Account Panel")] 
        [SerializeField] private TextMeshProUGUI m_playerName;

        private GameManager GM;
        DataBaseController _dataBaseController;
        
        private void Start()
        {
            _dataBaseController = DataBaseController.Instance;
            GM = GameManager.Instance;
            UpdateInfo();
        }
        
        public void OnPlay()
        {
            if(!PhotonNetwork.InLobby)
                PhotonNetwork.JoinLobby();
        }
        
        private async void UpdateInfo()
        {
            m_playerName.text = "Loading...";
            await _dataBaseController.OnSync();
            
            m_playerName.text = GM.playerInfo.playerName;
        }
    }
}