using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
        private List<GameSaveDataBase> userId = new List<GameSaveDataBase>();

        private void Start()
        {
            if (is_singUp) SingUp();
            else SignIn();
        }

        private void Update()
        {
            if (_sessionTask != null)
            {
                _loadpercent = GetConnectionProgress();
                //print("Connet Progress : " + _loadpercent * 100 + "%");
            }

            if (Client.Instance != null && _client == null)
                _client = Client.Instance;

            if (_session != null)
            {
                //print($"ID is : {_session.User.Id}");
            }
        }

        public float GetConnectionProgress()
        {
            float progress = 0;

            if (_sessionTask.Status == TaskStatus.Created || _sessionTask.Status == TaskStatus.WaitingForActivation ||
                _sessionTask.Status == TaskStatus.WaitingToRun)
                progress = 0;
            else if (_sessionTask.Status == TaskStatus.Running)
                progress = 0.5f;
            else if (_sessionTask.Status == TaskStatus.WaitingForChildrenToComplete)
                progress = 0.9f;
            else if (_sessionTask.Status == TaskStatus.RanToCompletion)
                progress = 1;

            return progress;
        }

        private async void SingUp()
        {
            SupabaseOptions
                supabaseOptions = new SupabaseOptions()
                {
                    AutoConnectRealtime = true
                }; //(m_supaBaseURL, m_supaBaseKey, supabaseOptions, client1 => { CallBack(); });
            _client = await Client.InitializeAsync(m_supaBaseURL, m_supaBaseKey, supabaseOptions);

            _sessionTask = _client.Auth.SignUp(_email, _password);
            print("Connet Progress : " + _sessionTask.Status);
            try
            {
                await _sessionTask;
            }
            catch (BadRequestException badRequestException)
            {
                _result += "BadRequestException";
                _result += $"{badRequestException.Message}";
                _result += $"{badRequestException.Content}";
                _result += $"{badRequestException.StackTrace}";
                return;
            }
            catch (UnauthorizedException unauthorizedException)
            {
                _result += "UnauthorizedException";
                _result += unauthorizedException.Message;
                _result += unauthorizedException.Content;
                _result += unauthorizedException.StackTrace;
                return;
            }
            catch (ExistingUserException existingUserException)
            {
                _result += "ExistingUserException";
                _result += existingUserException.Message;
                _result += existingUserException.Content;
                _result += existingUserException.StackTrace;
                return;
            }
            catch (ForbiddenException forbiddenException)
            {
                _result += "ForbiddenException";
                _result += forbiddenException.Message;
                _result += forbiddenException.Content;
                _result += forbiddenException.StackTrace;
                return;
            }
            catch (InvalidEmailOrPasswordException invalidEmailOrPasswordException)
            {
                _result += "invalidEmailOrPasswordException";
                _result += invalidEmailOrPasswordException.Message;
                _result += invalidEmailOrPasswordException.Content;
                _result += invalidEmailOrPasswordException.StackTrace;
                return;
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

                var set_model = new GameSaveDataBase();
                set_model.created_at = DateTime.Now;
                set_model.user_id = _session.User.Id;
                await _client.From<GameSaveDataBase>().Insert(set_model);
                
                var get_model = new GameSaveDataBase();
                get_model.user_id = _session.User.Id;
                get_model.created_at = DateTime.Now;
                var data = await _client.From<GameSaveDataBase>()
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

        private async void SignIn()
        {
            SupabaseOptions
                supabaseOptions = new SupabaseOptions()
                {
                    AutoConnectRealtime = true
                }; //(m_supaBaseURL, m_supaBaseKey, supabaseOptions, client1 => { CallBack(); });
            _client = await Client.InitializeAsync(m_supaBaseURL, m_supaBaseKey, supabaseOptions);

            _sessionTask = _client.Auth.SignIn(_email, _password);
            print("Connet Progress : " + _sessionTask.Status);
            try
            {
                await _sessionTask;
            }
            catch (BadRequestException badRequestException)
            {
                _result += "BadRequestException";
                _result += $"{badRequestException.Message}";
                _result += $"{badRequestException.Content}";
                _result += $"{badRequestException.StackTrace}";
                return;
            }
            catch (UnauthorizedException unauthorizedException)
            {
                _result += "UnauthorizedException";
                _result += unauthorizedException.Message;
                _result += unauthorizedException.Content;
                _result += unauthorizedException.StackTrace;
                return;
            }
            catch (ExistingUserException existingUserException)
            {
                _result += "ExistingUserException";
                _result += existingUserException.Message;
                _result += existingUserException.Content;
                _result += existingUserException.StackTrace;
                return;
            }
            catch (ForbiddenException forbiddenException)
            {
                _result += "ForbiddenException";
                _result += forbiddenException.Message;
                _result += forbiddenException.Content;
                _result += forbiddenException.StackTrace;
                return;
            }
            catch (InvalidEmailOrPasswordException invalidEmailOrPasswordException)
            {
                _result += "invalidEmailOrPasswordException";
                _result += invalidEmailOrPasswordException.Message;
                _result += invalidEmailOrPasswordException.Content;
                _result += invalidEmailOrPasswordException.StackTrace;
                return;
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
            
            //Select / Get Data
            var model = new GameSaveDataBase();
            model.user_id = _session.User.Id;
            model.created_at = DateTime.Now;
            var data = await _client.From<GameSaveDataBase>()
                .Filter("user_id", Constants.Operator.Equals, _session.User.Id)
                .Get();

            _result = $"Supabase sign in user id: {_session?.User?.Id}";
            foreach (var sdata in data.Models)
            {
                print("Connet Progress : " + sdata.user_id);
            }
            
            //Update Data
            var update_model = await _client
                .From<GameSaveDataBase>()
                .Filter("user_id", Constants.Operator.Equals, _session.User.Id)
                .Single();
            //Create Json Save File
            update_model.savedata = JsonConvert.SerializeObject(new GameInstance());

            await update_model.Update<GameSaveDataBase>();
        }

        public void CallBack()
        {
            print("Call Me by Your Name......");
        }
/*
        IEnumerator OnConnection(SupabaseOptions options, Func<Task<Client>> _funcTask)
        {
            Task.Run( async () =>
                    {
                        try
                        {
                            await _funcTask.Invoke();
                        }
                        catch (Exception e)
                        {
                            Debug.Log(e.Message);
                        }
                    }
            );

            if (_taskClient.Status == TaskStatus.Created || _taskClient.Status == TaskStatus.WaitingForActivation ||
                _taskClient.Status == TaskStatus.WaitingToRun)
                _loadpercent = 0;
            else if (_taskClient.Status == TaskStatus.Running)
                _loadpercent = 0.5f;
            else if (_taskClient.Status == TaskStatus.WaitingForChildrenToComplete)
                _loadpercent = 0.9f;
            else if (_taskClient.Status == TaskStatus.RanToCompletion)
                _loadpercent = 1;

            if(_taskClient.Status == TaskStatus.RanToCompletion)
                yield break;
        }*/

        private async void OnDisable()
        {
            await _client.Auth.SignOut();
        }
    }
}