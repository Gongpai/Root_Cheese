using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using GDD.JsonHelper;
using GDD.Sinagleton;
using Newtonsoft.Json.Linq;
using UnityEngine;

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

        public DataBaseManager dataBase
        {
            get => _dataBaseManager;
        }

        public IConnectionError ConnectionError
        {
            get => _connectionError;
            set => _connectionError = value;
        }
        
        private void Start()
        {
            GM = GameManager.Instance;
            _dataBaseManager.CreateClient(m_supaBaseURL, m_supaBaseKey);
        }

        public async Task SingUp(string email, string password, GameInstance instance)
        {
            //Error Action
            _dataBaseManager.errorAction -= SignInOnErrorAction;
            _dataBaseManager.errorAction -= SignUpOnErrorAction;
            _dataBaseManager.errorAction += SignUpOnErrorAction;
            
            JObject data = JsonHelperScript.CreateJsonObject<GameInstance>(instance);
            await _dataBaseManager.SingUp(email, password, data);
            await OnSync();
            GM.GI = _dataBaseManager.GetData<GameInstance>();
        }

        private void SignUpOnErrorAction()
        {
            print("Unable to register. Please check your information. and check the internet connection.");
            _connectionError!.OnErrorAction();
        }

        public async Task SignIn(string email, string password)
        {
            _dataBaseManager.errorAction -= SignUpOnErrorAction;
            _dataBaseManager.errorAction -= SignInOnErrorAction;
            _dataBaseManager.errorAction += SignInOnErrorAction;
            await _dataBaseManager.SignIn(email, password);
            GM.GI = _dataBaseManager.GetData<GameInstance>();
        }
        
        private void SignInOnErrorAction()
        {
            print("Incorrect email or password.");
            _connectionError!.OnErrorAction();
        }
        
        public async Task SignOut()
        {
            await _dataBaseManager.SignOut();
            GM.GI = _dataBaseManager.GetData<GameInstance>();
        }

        public async Task OnUpdate(GameInstance instance)
        {
            JObject data = JsonHelperScript.CreateJsonObject<GameInstance>(instance);
            await _dataBaseManager.Update(data);
            GM.GI = _dataBaseManager.GetData<GameInstance>();
        }

        public async Task OnSync()
        {
            await _dataBaseManager.SyncClientData();
            GM.GI = _dataBaseManager.GetData<GameInstance>();
        }

        public float GetProgress()
        {
            return _dataBaseManager.progress;
        }
        
        private async void OnDisable()
        {
            await _dataBaseManager.SignOut();
            _dataBaseManager.RemoveClient();
        }
    }
}