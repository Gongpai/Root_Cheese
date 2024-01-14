using System;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class ReadyCheckUI : MonoBehaviour
    {
        [SerializeField] private Image m_icon;
        [SerializeField] private Sprite m_readyIcon;
        [SerializeField] private Sprite m_notReadyIcon;
        [SerializeField] private bool m_ready;

        public bool ready
        {
            get => m_ready;
            set => m_ready = value;
        }

        private void Start()
        {
            m_icon.sprite = m_notReadyIcon;
        }

        private void Update()
        {
            if (m_ready)
                m_icon.sprite = m_readyIcon;
            else
                m_icon.sprite = m_notReadyIcon;
        }
    }
}