using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class TutorialsUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_text;
        [SerializeField] private Image m_TutorialsImage;
        [SerializeField] private GameObject m_spawnButton;
        [SerializeField] private List<TutorialsPresets> m_tutorialsPresets;
        [SerializeField] private GameObject m_ButtonImage;
        private int currentPage;
        private List<GameObject> m_buttons = new List<GameObject>();

        private void OnEnable()
        {
            CreateTutorialsPresets(0);
        }

        public void OpenTutorialsPage(int page)
        {
            currentPage += page;

            if (currentPage >= m_tutorialsPresets.Count - 1)
                currentPage = m_tutorialsPresets.Count - 1;
            else if (currentPage <= 0)
                currentPage = 0;
            
            CreateTutorialsPresets(currentPage);
        }
        
        public void CreateTutorialsPresets(int index)
        {
            foreach (var vButton in m_buttons)
            {
                Destroy(vButton);
            }
            
            m_buttons = new List<GameObject>();
            
            foreach (var button in m_tutorialsPresets[index].buttons)
            {
                GameObject spawnButton = Instantiate(m_ButtonImage);
                m_buttons.Add(spawnButton);
                spawnButton.GetComponent<RectTransform>().SetParent(m_spawnButton.transform);
                spawnButton.GetComponent<Image>().sprite = button;
                spawnButton.transform.localScale = Vector3.one;
            }

            m_text.text = m_tutorialsPresets[index].text;
            m_TutorialsImage.sprite = m_tutorialsPresets[index].tutorials;

            currentPage = index;
        }
    }
}