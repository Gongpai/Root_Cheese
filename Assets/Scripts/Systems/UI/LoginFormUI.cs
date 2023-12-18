using GDD.DataBase;
using TMPro;
using UnityEngine;
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
        [SerializeField] private DataBaseConnecter _dataConnecter;
        
        
    }
}