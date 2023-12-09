using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace GDD
{
    public class ScrollViewForAnimation : MonoBehaviour
    {
        [SerializeField]
        private RectTransform m_content;
        [Range(0, 1)]
        public float m_scollDelta;
        private float _height;
        
        private void Update()
        {
            if (m_content != null)
            {
                _height = m_content.sizeDelta.y - m_content.transform.parent.GetComponent<RectTransform>().rect.height;
                //print(Mathf.Lerp(0, _height, m_scollDelta));

                m_content.anchoredPosition = new Vector2(0, Mathf.Lerp(0, _height, m_scollDelta));
            }
        }
    }
}