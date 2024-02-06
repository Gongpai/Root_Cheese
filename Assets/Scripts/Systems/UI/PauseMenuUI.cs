using System;
using GDD.PUN;
using Photon.Pun;
using UnityEngine;

namespace GDD
{
    public class PauseMenuUI : MonoBehaviour
    {
        [SerializeField] private string m_nameMainMenu;
        private GameManager GM;
        private PunLevelManager PLM;

        private void Start()
        {
            GM = GameManager.Instance;
            PLM = PunLevelManager.Instance;
        }

        public void OnBackToMainMenu()
        {
            PhotonNetwork.LoadLevel(m_nameMainMenu);
        }
        
        public void OnReOpenLevelScene()
        {
            PhotonNetwork.LoadLevel(PLM.openLevel);
        }
    }
}