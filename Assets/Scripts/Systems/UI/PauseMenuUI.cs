using System;
using GDD.PUN;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GDD
{
    public class PauseMenuUI : UI
    {
        [SerializeField] private string m_nameMainMenu;
        private GameManager GM;
        private PunLevelManager PLM;

        private void Start()
        {
            GM = GameManager.Instance;
            PLM = PunLevelManager.Instance;
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
            PhotonNetwork.LoadLevel(PLM.openLevel);
        }
    }
}