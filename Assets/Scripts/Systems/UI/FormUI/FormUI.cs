using System;
using GDD.DataBase;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GDD
{
    public class FormUI : UI, IConnectionError
    {
        [SerializeField] protected GameObject m_loading;
        [SerializeField] protected DataBaseController _dataBaseController;
        [SerializeField] protected TextMeshProUGUI m_errorText;
        
        protected GameManager GM;

        protected void OnEnable()
        {
            m_errorText.text = "";
        }

        protected virtual void Start()
        {
            GM = GameManager.Instance;
        }

        protected virtual void Update()
        {
            if (_dataBaseController.GetProgress() == 0)
                OnStopProcess();
            else if(_dataBaseController.GetProgress() == 0.1f)
                OnStartProcess();
            else if (_dataBaseController.GetProgress() == 1)
                OnEndProcess();
        }

        protected virtual void OnStopProcess()
        {
            m_loading.SetActive(false);
        }

        protected virtual void OnStartProcess()
        {
            m_loading.SetActive(true);
        }

        protected virtual void OnEndProcess()
        {
            m_loading.SetActive(false);
        }
        
        public virtual void OnErrorAction()
        {
            
        }

        public virtual void OnLogin()
        {
            _dataBaseController.ConnectionError = this;
        }

        public virtual void OnLoginGuest()
        {
            _dataBaseController.GuestSignIn();
        }

        public virtual void OnSignUp()
        {
            _dataBaseController.ConnectionError = this;
        }
    }
}