﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using GDD.JsonHelper;
using GDD.Sinagleton;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GDD.DataBase
{
    public class DataBaseController : DontDestroy_Singleton<DataBaseController>
    {
        [TextArea] 
        [SerializeField] 
        private string m_supaBaseURL = "https://cevgbpclgngixiswbzsx.supabase.co";
        [TextArea] 
        [SerializeField] 
        private string m_supaBaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImNldmdicGNsZ25naXhpc3dienN4Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MDI4MTUwMzksImV4cCI6MjAxODM5MTAzOX0.hmyg57-RqIKgYzv5Xrk7p9fMslXz4xlU1Pzn8vJUo6I";

        private GameManager GM;
        private DataBaseManager _dataBaseManager = new DataBaseManager();
        private IConnectionError _connectionError;
        private UnityAction _onSignUpSucceed;
        private UnityAction _onSignInSucceed;
        private UnityAction _onSignOutSucceed;
        private UnityAction _onUpdateSucceed;
        private UnityAction _onSyncSucceed;

        public DataBaseManager dataBase
        {
            get => _dataBaseManager;
        }

        public IConnectionError ConnectionError
        {
            get => _connectionError;
            set => _connectionError = value;
        }

        public UnityAction OnSignUpSucceed
        {
            get => _onSignUpSucceed;
            set => _onSignUpSucceed = value;
        }

        public UnityAction OnSignInSucceed
        {
            get => _onSignInSucceed;
            set => _onSignInSucceed = value;
        }
        
        public UnityAction OnSignOutSucceed
        {
            get => _onSignOutSucceed;
            set => _onSignOutSucceed = value;
        }
        public UnityAction OnUpdateSucceed
        {
            get =>  _onUpdateSucceed;
            set => _onUpdateSucceed = value;
        }
        public UnityAction OnSyncSucceed
        {
            get =>  _onSyncSucceed;
            set => _onSyncSucceed = value;
        }
        
        private void Start()
        {
            GM = GameManager.Instance;
            _dataBaseManager.CreateClient(m_supaBaseURL, m_supaBaseKey);
        }

        public async Task SingUp(string email, string password, PlayerInfo playerInfo)
        {
            if(_dataBaseManager.client == null)
                _dataBaseManager.CreateClient(m_supaBaseURL, m_supaBaseKey);
            
            if(GM == null)
                GM = GameManager.Instance;
            
            //Error Action
            _dataBaseManager.errorAction -= SignInOnErrorAction;
            _dataBaseManager.errorAction -= SignUpOnErrorAction;
            _dataBaseManager.errorAction += SignUpOnErrorAction;

            JObject[] jObjects = new JObject[]
            {
                JsonHelperScript.CreateJsonObject<PlayerInfo>(playerInfo),
                JsonHelperScript.CreateJsonObject<GameInstance>(GM.gameInstance)
            };
            await _dataBaseManager.SingUp(email, password, jObjects);
            _onSignUpSucceed?.Invoke();
            await OnSync();
            GM.playerInfo = _dataBaseManager.GetData<PlayerInfo>(_dataBaseManager.data.playerInfo);
        }

        private void SignUpOnErrorAction()
        {
            print("Unable to register. Please check your information. and check the internet connection.");
            _connectionError!.OnErrorAction();
        }

        public async Task SignIn(string email, string password)
        {
            if(GM == null)
                GM = GameManager.Instance;
            
            if(_dataBaseManager.client == null){
                _dataBaseManager.CreateClient(m_supaBaseURL, m_supaBaseKey);}
            
            _dataBaseManager.errorAction -= SignUpOnErrorAction;
            _dataBaseManager.errorAction -= SignInOnErrorAction;
            _dataBaseManager.errorAction += SignInOnErrorAction;
            await _dataBaseManager.SignIn(email, password);
            _onSignInSucceed?.Invoke();
            GM.playerInfo = _dataBaseManager.GetData<PlayerInfo>(_dataBaseManager.data.playerInfo);
        }

        public void GuestSignIn()
        {
            if(GM == null)
                GM = GameManager.Instance;
            
            if(_dataBaseManager.client == null)
                _dataBaseManager.CreateClient(m_supaBaseURL, m_supaBaseKey);
            
            object[] data = new object[]
            {
                "",
                GM.gameInstance
            };
            _dataBaseManager.GuestSignIn(data);
            GM.playerInfo = _dataBaseManager.GetData<PlayerInfo>(_dataBaseManager.data.playerInfo);
            GM.gameInstance = _dataBaseManager.GetData<GameInstance>(_dataBaseManager.data.gameSave);
        }
        
        private void SignInOnErrorAction()
        {
            print("Incorrect email or password.");
            _connectionError!.OnErrorAction();
        }
        
        public async Task SignOut()
        {
            if(GM == null)
                GM = GameManager.Instance;
            
            await _dataBaseManager.SignOut();
            _onSignOutSucceed?.Invoke();
            GM.playerInfo = _dataBaseManager.GetData<PlayerInfo>(_dataBaseManager.data.playerInfo);
        }

        public async Task OnUpdate(PlayerInfo playerInfo, GameInstance gameInstance)
        {
            if(GM == null)
                GM = GameManager.Instance;
            
            object[] data = new object[]
            {
                playerInfo,
                gameInstance
            };
            await _dataBaseManager.Update(data);
            _onUpdateSucceed?.Invoke();
            print($"Update Succeed!");
            GM.playerInfo = _dataBaseManager.GetData<PlayerInfo>(_dataBaseManager.data.playerInfo);
            GM.gameInstance = _dataBaseManager.GetData<GameInstance>(_dataBaseManager.data.gameSave);
        }

        public async Task OnSync()
        {
            if(GM == null)
                GM = GameManager.Instance;
            
            print("Begin Sync...");
            await _dataBaseManager.SyncClientData();
            print("Sync End...");
            _onSyncSucceed?.Invoke();
            print($"Invoke Succeed!!");
            GM.playerInfo = _dataBaseManager.GetData<PlayerInfo>(_dataBaseManager.data.playerInfo);
            GM.gameInstance = _dataBaseManager.GetData<GameInstance>(_dataBaseManager.data.gameSave);
            print("Sync Succ!!!!!");
        }

        public float GetProgress()
        {
            return _dataBaseManager.progress;
        }
        
        private async void OnDisable()
        {
            
        }

        private async void OnApplicationQuit()
        {
            Debug.LogWarning("Destroy D B C");
            await _dataBaseManager.SignOut();
            _dataBaseManager.RemoveClient();
        }
    }
}