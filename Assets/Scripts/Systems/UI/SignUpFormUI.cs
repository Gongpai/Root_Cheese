﻿using System;
using GDD.DataBase;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GDD
{
    public class SignUpFormUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField m_e_mail;
        [SerializeField] private TMP_InputField m_password;
        [SerializeField] private TMP_InputField m_name;
        [SerializeField] private TMP_InputField m_age;
        [SerializeField] private TMP_InputField m_birthday;
        [SerializeField] private Button m_login;
        [SerializeField] private Button m_register;
        [SerializeField] private GameObject m_loading;
        [SerializeField] private DataBaseController _dataBaseController;

        private GameManager GM;
        private void Start()
        {
            GM = GameManager.Instance;
        }

        private void Update()
        {
            if(_dataBaseController.GetProgress() == 0.1f)
                m_loading.SetActive(true);
            else if (_dataBaseController.GetProgress() == 1)
            {
                m_loading.SetActive(false);
            }
        }

        public async void OnSignUp()
        {
            print($"E-mail : {m_e_mail.text} | Password : {m_password.text}");
            if (m_e_mail.text != "" && m_password.text != "" && m_age.text != "" && m_birthday.text != "" && m_name.text != "")
            {
                GameInstance gameInstance = new GameInstance();
                gameInstance.playerName = m_name.text;
                gameInstance.age = int.Parse(m_age.text);
                gameInstance.date = m_birthday.text;
                await _dataBaseController.SingUp(m_e_mail.text, m_password.text, gameInstance);
                gameObject.SetActive(false);
                print("Login!");
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