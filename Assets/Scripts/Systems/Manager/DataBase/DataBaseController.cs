using System;
using System.Threading.Tasks;
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
        
        private void Start()
        {
            GM = GameManager.Instance;
            _dataBaseManager.CreateClient(m_supaBaseURL, m_supaBaseKey);
        }

        public async Task SingUp(string email, string password, GameInstance instance)
        {
            JObject data = JsonHelperScript.CreateJsonObject<GameInstance>(instance);
            await _dataBaseManager.SingUp(email, password, data);
            
        }

        public async Task SignIn(string email, string password)
        {
            await _dataBaseManager.SignIn(email, password);
            GM.GI = _dataBaseManager.GetData<GameInstance>();
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