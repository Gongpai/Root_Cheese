using System;
using GDD.DataBase;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class UserFormUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_username;
        [SerializeField] private TMP_InputField m_name;
        [SerializeField] private TMP_InputField m_age;
        [SerializeField] private TMP_InputField m_birthday;
        [SerializeField] private Button m_save;
        [SerializeField] private Button m_close;
        [SerializeField] private GameObject m_loading;
        [SerializeField] private DataBaseConnecter _dataConnecter;

        private void OnEnable()
        {
            UpdateInfo();
        }

        public async void OnUpdateSave()
        {
            print($"Age : {m_age.text} | Birthday : {m_birthday.text}");
            if (m_age.text != "" && m_birthday.text != "" && m_name.text != "")
            {
                GameInstance gameInstance = new GameInstance();
                gameInstance.playerName = m_name.text;
                gameInstance.age = int.Parse(m_age.text);
                gameInstance.date = m_birthday.text;
                await _dataConnecter.UpdateSave(gameInstance);
                
                print("Login!");
                
                UpdateInfo();
            }
            else
            {
                print("Text is Null");
            }
        }

        private async void UpdateInfo()
        {
            await _dataConnecter.SyncData();
            
            m_age.text = _dataConnecter.saveData.age.ToString();
            m_birthday.text = _dataConnecter.saveData.date;
            m_name.text = _dataConnecter.saveData.playerName;
            m_username.text = "Username : " + _dataConnecter.saveData.playerName;
        }
        
        public async void OnClose()
        {
            await _dataConnecter.SignOut();
            
            gameObject.SetActive(false);
            
        }
    }
}