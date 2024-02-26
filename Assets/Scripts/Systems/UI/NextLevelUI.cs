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

        private void Start()
        {
            m_animation = GetComponent<Animator>();
            GM = GameManager.Instance;

            if (m_UI == null)
                m_UI = transform.GetChild(0).gameObject;
            
            m_UI.SetActive(false);
        }

        private void Update()
        {
            if(GM.gameState == GameState.GameOver)
               m_UI.SetActive(true);
        }
    }
}