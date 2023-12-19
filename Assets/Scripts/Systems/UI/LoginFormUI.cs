using System;
using GDD.DataBase;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GDD
{
    public class LoginFormUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField m_e_mail;
        [SerializeField] private TMP_InputField m_password;
        [SerializeField] private Button m_login;
        [SerializeField] private Button m_register;
        [SerializeField] private GameObject m_loading;
        [SerializeField] private DataBaseController _dataBaseController;
        [SerializeField] private GameObject m_userForm;
        [SerializeField] private GameObject m_signUp;

        private void Update()
        {
            if(_dataBaseController.GetProgress() == 0.1f)
                m_loading.SetActive(true);
            else if (_dataBaseController.GetProgress() == 1)
            {
                m_loading.SetActive(false);
                m_userForm.SetActive(true);
            }
        }

        public void OnLogin()
        {
            print($"E-mail : {m_e_mail.text} | Password : {m_password.text}");
            if (m_e_mail.text != "" && m_password.text != "")
            {
                _dataBaseController.SignIn(m_e_mail.text, m_password.text);
                print("Login!");
            }
            else
            {
                print("E-mail or Password is Null");
            }
        }

        public void OnSignUp()
        {
            m_signUp.SetActive(true);
        }
    }
}