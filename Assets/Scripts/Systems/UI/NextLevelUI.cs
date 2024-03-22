using System;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class NextLevelUI : UI
    {
        [SerializeField] private GameObject m_UI;
        private GameManager GM;
        private Animator m_animation;

        protected override void Start()
        {
            base.Start();
            m_animation = GetComponent<Animator>();
            GM = GameManager.Instance;

            if (m_UI == null)
                m_UI = transform.GetChild(0).gameObject;
            
            m_UI.SetActive(false);
        }

        private void Update()
        {
            if(GM.gameState == GameState.Win)
               m_UI.SetActive(true);
            else
                m_UI.SetActive(false);
        }
    }
}