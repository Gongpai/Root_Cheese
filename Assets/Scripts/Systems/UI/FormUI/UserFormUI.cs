using System;
using GDD.DataBase;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
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
        [SerializeField] private DataBaseController _dataBaseController;

        private GameManager GM;
        private void OnEnable()
        {
            UpdateInfo();
        }

        private void Start()
        {
            GM = GameManager.Instance;
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
                await _dataBaseController.OnUpdate(gameInstance);
                
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
            await _dataBaseController.OnSync();
            
            m_age.text = GM.GI.age.ToString();
            m_birthday.text = GM.GI.date;
            m_name.text = GM.GI.playerName;
            m_username.text = "Username : " + GM.GI.playerName;
        }
        
        public async void OnClose()
        {
            await _dataBaseController.SignOut();
            
            gameObject.SetActive(false);
            
        }
    }
}