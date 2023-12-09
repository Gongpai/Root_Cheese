using System;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.UI
{
    public class ProgressBarScale : MonoBehaviour
    {
        [SerializeField][Range(0, 1)] private float m_fillAmount;
        [SerializeField] private float m_size = 250;
        private RectTransform m_rectTransform;
        private Image m_image;

        public float fillAmount
        {
            get => m_fillAmount;
            set => m_fillAmount = value;
        }

        private void Start()
        {
            m_rectTransform = GetComponent<RectTransform>();
            m_image = GetComponent<Image>();
        }

        private void Update()
        {
            if (m_fillAmount > 0.0f)
                m_image.color = Color.white;
            else
                m_image.color = new Color(0, 0, 0, 0);
            
            float pos_range = Mathf.Abs(m_size) / 2 - 3.5f;
            //m_rectTransform.anchoredPosition = new Vector2(Mathf.Lerp(-pos_range, pos_range, m_fillAmount), 0);
            m_rectTransform.anchoredPosition = new Vector2(Mathf.Lerp(-pos_range, 0, m_fillAmount), 0);
            m_rectTransform.sizeDelta = new Vector2(Mathf.Lerp(-m_size, -7, m_fillAmount), -6);
        }
    }
}