using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GDD
{
    public class SignInFormUI : FormUI
    {
        [Header("Login Setting")]
        [SerializeField] protected TMP_InputField m_e_mail;
        [SerializeField] protected TMP_InputField m_password;
        [SerializeField] protected Button m_login;
        [SerializeField] protected Button m_register;
        
        [Header("User Info")]
        [SerializeField] protected GameObject m_userForm;
        [SerializeField] protected GameObject m_signUp;

        [Header("Login Action")] 
        [SerializeField] private UnityEvent OnUserSignIn;
        [SerializeField] private UnityEvent OnUserSignUp;

        protected override void OnStopProcess()
        {
            base.OnStopProcess();
            m_userForm.SetActive(false);
        }

        protected override void OnStartProcess()
        {
            base.OnStartProcess();
        }

        protected override void OnEndProcess()
        {
            base.OnEndProcess();
            OnUserSignIn?.Invoke();
        }

        public override void OnErrorAction()
        {
            base.OnErrorAction();
            m_errorText.text = "Incorrect email or password.";
        }

        public override void OnLogin()
        {
            base.OnLogin();
            
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

        public override void OnSignUp()
        {
            m_errorText.text = "";
            OnUserSignUp?.Invoke();
            m_signUp.SetActive(true);
        }
    }
}