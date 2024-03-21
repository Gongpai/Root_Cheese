using System;
using GDD.DataBase;
using GDD.PUN;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public class PauseMainMenuUI : PauseMenuUI
    {
        [SerializeField] protected GameObject m_loading;
        [SerializeField] private UnityEvent m_signOut;
        private DataBaseController _dataBaseController;

        private void OnEnable()
        {
            m_loading.SetActive(false);
        }

        protected override void Start()
        {
            base.Start();
            
            _dataBaseController = DataBaseController.Instance;
        }
        
        public async void OnSignOut()
        {
            m_loading.SetActive(true);
            print($"SignOut");
            
            if(_dataBaseController != null)
                await _dataBaseController.SignOut();
            
            GM.ResetGameInstance();
            PhotonNetwork.Disconnect();
            
            if(_dataBaseController != null )
                Destroy(_dataBaseController.gameObject);
            
            if(PunNetworkManager.Instance != null)
                Destroy(PunNetworkManager.Instance.gameObject);
            
            m_signOut?.Invoke();
        }
    }
}