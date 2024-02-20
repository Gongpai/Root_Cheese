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
            _dataBaseController = DataBaseController.Instance;
            GM = GameManager.Instance;
            UpdateInfo();
        }

        private void Start()
        {
            
        }

        public async void OnUpdateSave()
        {
            print($"Age : {m_age.text} | Birthday : {m_birthday.text}");
            if (m_age.text != "" && m_birthday.text != "" && m_name.text != "")
            {
                PlayerInfo playerinfo = new PlayerInfo();
                playerinfo.playerName = m_name.text;
                playerinfo.age = int.Parse(m_age.text);
                playerinfo.date = m_birthday.text;
                await _dataBaseController.OnUpdate(playerinfo, GM.gameInstance);
                
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
            print($"DataBase Is Null : {_dataBaseController == null}");
            await _dataBaseController.OnSync();
            
            m_age.text = GM.playerInfo.age.ToString();
            m_birthday.text = GM.playerInfo.date;
            m_name.text = GM.playerInfo.playerName;
            m_username.text = "Username : " + GM.playerInfo.playerName;
        }
        
        public async void OnClose()
        {
            await _dataBaseController.SignOut();
            
            gameObject.SetActive(false);
            
        }
    }
}