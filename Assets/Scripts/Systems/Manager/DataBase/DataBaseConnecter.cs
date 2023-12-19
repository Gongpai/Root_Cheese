using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Postgrest;
using Supabase;
using Supabase.Gotrue;
using UnityEngine;
using Supabase.Realtime;
using Client = Supabase.Client;
using Constants = Postgrest.Constants;

namespace GDD.DataBase
{
    public class DataBaseConnecter : MonoBehaviour
    {
        [SerializeField] private bool is_singUp = true;

        [TextArea] [SerializeField] private string m_supaBaseURL = "https://cevgbpclgngixiswbzsx.supabase.co";

        [TextArea] [SerializeField] private string m_supaBaseKey =
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImNldmdicGNsZ25naXhpc3dienN4Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MDI4MTUwMzksImV4cCI6MjAxODM5MTAzOX0.hmyg57-RqIKgYzv5Xrk7p9fMslXz4xlU1Pzn8vJUo6I";

        [SerializeField] private string _email;
        [SerializeField] private string _password;

        [TextArea] [SerializeField] private string _result;
        private float _loadpercent;
        private Task<Session> _sessionTask;
        private Client _client;
        private Session _session;
        private GameInstance _saveData = new();

        public GameInstance saveData
        {
            get => _saveData;
            set => _saveData = value;
        }

        public float progress
        {
            get => _loadpercent;
        }

        private void Start()
        {
            SupabaseOptions
                supabaseOptions = new SupabaseOptions()
                {
                    AutoConnectRealtime = true
                }; 
            _client = new Client(m_supaBaseURL, m_supaBaseKey, supabaseOptions);
            
            //For Debug
            /*
            if (is_singUp) SingUp();
            else SignIn();
            */
        }

        private void Update()
        {
            _loadpercent = GetConnectionProgress();

            if (_session != null)
            {
                //print($"ID is : {_session.User.Id}");
            }
        }

        public float GetConnectionProgress()
        {
            float progress = 0;

            if (_sessionTask != null)
            {
                if (_sessionTask.Status == TaskStatus.Created ||
                    _sessionTask.Status == TaskStatus.WaitingForActivation ||
                    _sessionTask.Status == TaskStatus.WaitingToRun)
                    progress = 0.1f;
                else if (_sessionTask.Status == TaskStatus.Running)
                    progress = 0.5f;
                else if (_sessionTask.Status == TaskStatus.WaitingForChildrenToComplete)
                    progress = 0.9f;
                else if (_sessionTask.Status == TaskStatus.RanToCompletion)
                    progress = 1;
                else
                    progress = 0;
            }
            else
            {
                progress = 0;
            }

            return progress;
        }

        public async Task SingUp(string email, string password, GameInstance instance)
        {
            await _client.InitializeAsync();

            _sessionTask = _client.Auth.SignUp(email, password);
            //_sessionTask = _client.Auth.SignUp(_email, _password);
            print("Connet Progress : " + _sessionTask.Status);
            try
            {
                await _sessionTask;
            }
            catch (Exception exception)
            {
                _result += "unknown exception";
                _result += exception.Message;
                _result += exception.StackTrace;
                return;
            }

            _session = _sessionTask.Result;
            if (_session == null)
                _result = "Failed";
            else
            {
                _result = $"Supabase sign in user id: {_session?.User?.Id}";

                InsertRowData set_model = new InsertRowData();
                set_model.created_at = DateTime.Now;
                string j_data = JsonConvert.SerializeObject(instance);
                JObject j_obj = JsonConvert.DeserializeObject<JObject>(j_data);
                set_model.savedata = j_obj;
                set_model.user_id = _session.User.Id;
                await _client.From<InsertRowData>().Insert(set_model);
                
                
                var data = await _client.From<UpdateRowData>()
                    .Filter("user_id", Constants.Operator.Equals, _session.User.Id)
                    .Get();
                //await _client.From<GameSaveDataBase>().Insert(model);

                _result = $"Supabase sign in user id: {_session?.User?.Id}";
                foreach (var sdata in data.Models)
                {
                    print("Connet Progress : " + sdata.user_id);
                }
            }


        }

        public async Task SignIn(string email, string password)
        {
            await _client .InitializeAsync();

            _sessionTask = _client.Auth.SignIn(email, password);
            //_sessionTask = _client.Auth.SignIn(_email, _password);
            print("Connet Progress : " + _sessionTask.Status);
            try
            {
                await _sessionTask;
            }
            catch (Exception exception)
            {
                _result += "unknown exception";
                _result += exception.Message;
                _result += exception.StackTrace;
                return;
            }

            _session = _sessionTask.Result;

            if (_session == null)
                _result += "unsuccessful";
            else
            {
                _result = $"Sign in success {_session.User?.Id} {_session.AccessToken} {_session.User?.Aud} {_session.User?.Email} {_session.RefreshToken}";
            }

            print("Connet Progress : " + _sessionTask.Status);
        }

        public void CallBack()
        {
            print("Call Me by Your Name......");
        }

        public async Task UpdateSave(JObject jObject)
        {
            //Update Data
            //Create Json Save File
            var update_model = await _client.From<UpdateRowData>()
                .Where(x => x.user_id == _session.User.Id)
                .Single();
            
            print($"SaveData : {update_model.savedata}");
            update_model.savedata = jObject;
            await update_model.Update<UpdateRowData>();
            
            print($"SaveData : {update_model.savedata}");
        }

        public async Task SyncData()
        {
            //Select / Get Data
            var data = await _client.From<UpdateRowData>()
                .Filter("user_id", Constants.Operator.Equals, _session.User.Id)
                .Single();

            _result = $"Supabase sign in user id: {_session?.User?.Id}";
            string sdata = JsonConvert.SerializeObject(data.savedata);
            _saveData = JsonConvert.DeserializeObject<GameInstance>(sdata);
            
            return;
        }
        
        public async Task SignOut()
        {
            _sessionTask = null;
            
            await _client.Auth.SignOut();
        }

        private async void OnDisable()
        {
            await _client.Auth.SignOut();
        }
    }
}