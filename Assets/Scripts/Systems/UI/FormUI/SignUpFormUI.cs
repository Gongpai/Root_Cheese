using System;
using GDD.DataBase;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GDD
{
    public class SignUpFormUI : FormUI
    {
        [SerializeField] private TMP_InputField m_e_mail;
        [SerializeField] private TMP_InputField m_password;
        [SerializeField] private TMP_InputField m_name;
        [SerializeField] private TMP_InputField m_age;
        [SerializeField] private TMP_InputField m_birthday;
        [SerializeField] private Button m_login;
        [SerializeField] private Button m_register;
        
        protected override void OnStopProcess()
        {
            base.OnStopProcess();
        }

        protected override void OnStartProcess()
        {
            base.OnStartProcess();
        }

        protected override void OnEndProcess()
        {
            base.OnEndProcess();
        }

        public override void OnErrorAction()
        {
            base.OnErrorAction();
            m_errorText.text = "Unable to register. Please check your information. and check the internet connection.";
        }

        public override void OnLogin()
        {
            
        }

        public override async void OnSignUp()
        {
            base.OnSignUp();
            
            print($"E-mail : {m_e_mail.text} | Password : {m_password.text}");
            if (m_e_mail.text != "" && m_password.text != "" && m_age.text != "" && m_birthday.text != "" && m_name.text != "")
            {
                PlayerInfo playerinfo = new PlayerInfo();
                playerinfo.playerName = m_name.text;
                playerinfo.age = int.Parse(m_age.text);
                playerinfo.date = m_birthday.text;
                await _dataBaseController.SingUp(m_e_mail.text, m_password.text, playerinfo);

                print($"Connect State : {_dataBaseController.dataBase.state}");
                if (_dataBaseController.dataBase.state == ConnectionState.Successfully)
                {
                    OnClose();
                    print("Login!");
                }
            }
            else
            {
                print("E-mail or Password is Null");
            }
        }
        
        public void OnClose()
        {
            gameObject.SetActive(false);
        }
    }
}