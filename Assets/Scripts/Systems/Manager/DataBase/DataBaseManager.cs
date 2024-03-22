using System;
using System.Collections.Generic;
using System.Globalization;
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
        private bool isGuest;

        public delegate void Action();
        public event Action errorAction;
        
        public Client client
        {
            get => _client;
        }

        public string result
        {
            get => _result;
        }

        public ConnectionState state
        {
            get;
            private set;
        }
        
        public float progress
        {
            get
            {
                if (isGuest)
                {
                    return 1;
                }
                else
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
            _result = "Client created";
            Debug.Log(_result);
            return _client;
        }

        //SignUp
        public async Task SingUp(string email, string password, JObject[] jObjects)
        {
            if (_client != null)
                await _client.InitializeAsync();
            else
            {
                _result = "Client not found or Client has not been created.";
                Debug.LogError(_result);
                return;
            }
            
            //SignUp new account
            state = ConnectionState.Start;
            _sessionTask = _client.Auth.SignUp(email, password);
            _result = $"Connect Status : {_sessionTask.Status}";
            Debug.Log(_result);
            try
            {
                await _sessionTask;
            }
            catch (Exception exception)
            {
                state = ConnectionState.Error;
                _result = "";
                _result += exception.Message;
                _result += exception.StackTrace;
                _session = null;
                _sessionTask = null;
                Debug.LogError(_result);
                errorAction?.Invoke();
                return;
            }
            
            //Set Sesstion
            _session = _sessionTask.Result;
            if (_session == null)
            {
                state = ConnectionState.Error;
                _result = "Sign up failed";
                Debug.LogError(_result);
            }
            else
            {
                state = ConnectionState.Successfully;
                _result = $"Supabase sign up user id: {_session?.User?.Id}";
                Debug.Log(_result);

                //Insert Data
                InsertRowData rowData = new InsertRowData();
                rowData.created_at = DateTime.Now;
                rowData.user_id = _session.User.Id;
                rowData.playerInfo = jObjects[0];
                rowData.gameSave = jObjects[1];
                await _client.From<InsertRowData>().Insert(rowData);
                
                //Get Data From DataBase
                var get_data = await _client.From<UpdateRowData>()
                    .Filter("user_id", Constants.Operator.Equals, _session.User.Id)
                    .Single();
                _data = get_data;
                
                _result = $"Successfully sign up user id: {_session?.User?.Id}";
                Debug.Log(_result);
            }
        }

        //SignIn
        public async Task SignIn(string email, string password)
        {
            if (isGuest)
            {
                _result = "You are logged in as a guest.";
                Debug.LogWarning(_result);
                return;
            }
            if (_client != null)
                await _client.InitializeAsync();
            else
            {
                _result = "Client not found or Client has not been created.";
                Debug.LogError(_result);
                return;
            }
            
            //SignIn Account
            state = ConnectionState.Start;
            _sessionTask = _client.Auth.SignIn(email, password);
            _result = $"Connect Status : {_sessionTask.Status}";
            Debug.Log(_result);
            try
            {
                await _sessionTask;
            }
            catch (Exception exception)
            {
                state = ConnectionState.Error;
                _result = "";
                _result += exception.Message;
                _result += exception.StackTrace;
                _session = null;
                _sessionTask = null;
                Debug.LogError(_result);
                errorAction?.Invoke();
                return;
            }
            
            //Set Sesstion
            _session = _sessionTask.Result;
            if (_session == null)
            {
                state = ConnectionState.Error;
                _result = "Sign in failed";
                Debug.LogError(_result);
            }
            else
            {
                state = ConnectionState.Successfully;
                _result = $"Successfully sign in user id: {_session.User?.Id} {_session.AccessToken} {_session.User?.Aud} {_session.User?.Email} {_session.RefreshToken}";
                Debug.Log(_result);
            }
        }

        //Guest SignIn
        public void GuestSignIn(object[] updateData)
        {
            _data = new InsertRowData();
            PlayerInfo playerInfo = new PlayerInfo();
            playerInfo.playerName = $"Guest {DateTime.Now.Millisecond}{DateTime.Now.Minute}";
            playerInfo.date = DateTime.Now.ToString();
            playerInfo.age = 1;
            _data.playerInfo = playerInfo;
            _data.user_id = "";
            _data.created_at = DateTime.Now;
            _data.gameSave = updateData[1];

            Debug.LogWarning($"Player Inf : {_data.playerInfo}");
            isGuest = true;
        }

        //Update row
        public async Task Update(object[] updateData)
        {
            if (isGuest)
            {
                _result = "You are logged in as a guest.";
                Debug.LogWarning(_result);
                _data.gameSave = updateData[1];
                return;
            }
            if (_client == null)
            {
                _result = "Client not found or Client has not been created.";
                Debug.LogError(_result);
                return;
            }
            
            var update_model = await _client.From<UpdateRowData>()
                .Where(x => x.user_id == _session.User.Id)
                .Single();
            
            //Set model
            update_model.playerInfo = JsonConvert.SerializeObject(updateData[0]);
            update_model.gameSave = JsonConvert.SerializeObject(updateData[1]);
            ThaiBuddhistCalendar thaiBuddhistCalendar = new ThaiBuddhistCalendar();
            DateTime currentDateTime = data.created_at;
            update_model.created_at = thaiBuddhistCalendar.ToDateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, currentDateTime.Hour, currentDateTime.Minute, currentDateTime.Second, currentDateTime.Millisecond);
            
            await update_model.Update<UpdateRowData>();
            
            _result = $"playerInfo : {update_model.playerInfo} / {update_model.gameSave}";
            Debug.Log($"playerInfo : {update_model.playerInfo} / {update_model.gameSave}");
        }

        //SyncData
        public async Task SyncClientData()
        {
            if (isGuest)
            {
                Debug.LogWarning($"Nulllll : {_data.gameSave == null} || {JsonConvert.SerializeObject(_data.gameSave)}");
                _result = "You are logged in as a guest.";
                Debug.LogWarning(_result);
                return;
            }
            if (_client == null)
            {
                _result = "Client not found or Client has not been created.";
                Debug.LogError(_result);
                return;
            }
            
            //Select / Get Data
            var data = await _client.From<UpdateRowData>()
                .Filter("user_id", Constants.Operator.Equals, _session.User.Id)
                .Single();
            
            _result = $"Successfully sync data user id: {_session?.User?.Id}";
            Debug.Log(_result);
            _data = data;
            Debug.LogWarning($"Player Inf : {_data.playerInfo}");
        }

        public T GetData<T>(object data)
        {
            return JsonHelperScript.ConvertTo<T>(data);
        }
        
        //SignOut
        public async Task SignOut()
        {
            if (isGuest)
            {
                _result = "You are logged in as a guest.";
                Debug.LogWarning(_result);
                _data = new UpdateRowData();
                return;
            }
            if (_client == null)
            {
                _result = "Client not found or Client has not been created.";
                Debug.LogError(_result);
                state = ConnectionState.Close;
                return;
            }
            
            //SignOut / Clear session
            _sessionTask = null;
            await _client.Auth.SignOut();

            if (_session != null)
            {
                _result = $"Successfully sign out user id: {_session.User?.Id}";
                Debug.Log(_result);
            }

            _session = null;
            isGuest = false;
        }

        public void RemoveClient()
        {
            _client = null;
            _result = "Client deleted";
            isGuest = false;
            Debug.Log(_result);
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