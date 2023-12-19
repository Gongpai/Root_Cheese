using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GDD.JsonHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Supabase;
using Supabase.Gotrue;
using UnityEngine;
using Client = Supabase.Client;
using Constants = Postgrest.Constants;

namespace GDD.DataBase
{
    public class DataBaseManager
    {
        private Client _client;
        private string _result;
        private Task<Session> _sessionTask;
        private Session _session;
        private ITableData _data;

        public Client client
        {
            get => _client;
        }

        public string result
        {
            get => _result;
        }
        
        public float progress
        {
            get
            {
                if (_sessionTask != null)
                {
                    if (_sessionTask.Status == TaskStatus.Created ||
                        _sessionTask.Status == TaskStatus.WaitingForActivation ||
                        _sessionTask.Status == TaskStatus.WaitingToRun)
                        return 0.1f;
                    else if (_sessionTask.Status == TaskStatus.Running)
                        return 0.5f;
                    else if (_sessionTask.Status == TaskStatus.WaitingForChildrenToComplete)
                        return 0.9f;
                    else if (_sessionTask.Status == TaskStatus.RanToCompletion)
                        return 1;
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }

                return 0;
            }
        }

        public ITableData data
        {
            get => _data;
        }

        //Create Client
        public Client CreateClient(string supaBaseURL, string supaBaseKey)
        {
            SupabaseOptions supabaseOptions = new SupabaseOptions()
                {
                    AutoConnectRealtime = true
                }; 
            
            _client = new Client(supaBaseURL, supaBaseKey, supabaseOptions);
            _client.Auth.AddDebugListener(DebugListener!);
            return _client;
        }

        //SignUp
        public async Task SingUp(string email, string password, JObject jObject)
        {
            if (_client != null)
                await _client.InitializeAsync();
            else
            {
                _result = "Client not found or Client has not been created.";
                return;
            }
            
            //SignUp new account
            _sessionTask = _client.Auth.SignUp(email, password);
            _result = $"Connect Status : {_sessionTask.Status}";
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
            
            //Set Sesstion
            _session = _sessionTask.Result;
            if (_session == null)
                _result = "Sign up failed";
            else
            {
                _result = $"Supabase sign up user id: {_session?.User?.Id}";

                //Insert Data
                InsertRowData rowData = new InsertRowData();
                rowData.created_at = DateTime.Now;
                rowData.user_id = _session.User.Id;
                rowData.savedata = jObject;
                await _client.From<InsertRowData>().Insert(rowData);
                
                //Get Data From DataBase
                var get_data = await _client.From<UpdateRowData>()
                    .Filter("user_id", Constants.Operator.Equals, _session.User.Id)
                    .Single();
                _data = get_data;
                
                _result = $"Supabase sign in user id: {_session?.User?.Id}";
            }
        }

        //SignIn
        public async Task SignIn(string email, string password)
        {
            if (_client != null)
                await _client.InitializeAsync();
            else
            {
                _result = "Client not found or Client has not been created.";
                return;
            }
            
            //SignIn Account
            _sessionTask = _client.Auth.SignIn(email, password);
            _result = $"Connect Status : {_sessionTask.Status}";
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
            
            //Set Sesstion
            _session = _sessionTask.Result;
            if (_session == null)
                _result = "Sign in failed";
            else
            {
                _result = $"Sign in success {_session.User?.Id} {_session.AccessToken} {_session.User?.Aud} {_session.User?.Email} {_session.RefreshToken}";
            }
        }

        //Update row
        public async Task Update(JObject jObject)
        {
            if (_client == null)
            {
                _result = "Client not found or Client has not been created.";
                return;
            }
            
            var update_model = await _client.From<UpdateRowData>()
                .Where(x => x.user_id == _session.User.Id)
                .Single();
            
            //Set model
            update_model.savedata = jObject;
            
            await update_model.Update<UpdateRowData>();
            
            _result = ($"SaveData : {update_model.savedata}");
            Debug.Log($"SaveData : {update_model.savedata}");
        }

        //SyncData
        public async Task SyncClientData()
        {
            if (_client == null)
            {
                _result = "Client not found or Client has not been created.";
                return;
            }
            
            //Select / Get Data
            var data = await _client.From<UpdateRowData>()
                .Filter("user_id", Constants.Operator.Equals, _session.User.Id)
                .Single();

            _result = $"Supabase sign in user id: {_session?.User?.Id}";
            _data = data;
        }

        public T GetData<T>()
        {
            return JsonHelperScript.ConvertTo<T>(_data.savedata);
        }
        
        //SignOut
        public async Task SignOut()
        {
            if (_client == null)
            {
                _result = "Client not found or Client has not been created.";
                return;
            }
            
            //SignOut / Clear session
            _sessionTask = null;
            await _client.Auth.SignOut();
        }

        public void RemoveClient()
        {
            _client = null;
        }
        
        private void DebugListener(string message, Exception e)
        {
            _result = message;
            Debug.Log(message);
            
            if (e != null)
                Debug.LogException(e);
        }
    }
}