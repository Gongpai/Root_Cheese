using GDD.DataBase;
using GDD.PUN;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public class PauseMainMenuUI : PauseMenuUI
    {
        [SerializeField] private UnityEvent m_signOut;
        private DataBaseController _dataBaseController;

        protected override void Start()
        {
            base.Start();
            
            _dataBaseController = DataBaseController.Instance;
        }
        
        public async void OnSignOut()
        {
            await _dataBaseController.SignOut();
            GM.ResetGameInstance();
            PhotonNetwork.Disconnect();
            Destroy(_dataBaseController.gameObject);
            Destroy(PunNetworkManager.Instance.gameObject);
            m_signOut?.Invoke();
        }
    }
}