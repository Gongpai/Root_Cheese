using System;
using TMPro;
using UnityEngine;

namespace GDD
{
    public class RandomSkill_UI : MonoBehaviour
    {
        [Header("Add Random Skill To Viewport")]
        [SerializeField] private GameObject m_viewport;
        
        [Header("Prefab Create Skill Button")]
        [SerializeField] private GameObject m_skill_Gruop;
        [SerializeField] private GameObject m_skill_Element;

        [Header("Prefab Random")] [SerializeField]
        private GameObject m_preRandom;

        [SerializeField] private RandomSkill _randomSkill;
        [SerializeField] private ScrollViewForAnimation _scrollViewAnim;
        
        private void Start()
        {
            _scrollViewAnim.enabled = false;
            _randomSkill = GetComponent<RandomSkill>();

            GameObject group = Instantiate(m_skill_Gruop);
            group.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            _randomSkill.OnRandomSkill(RandomSkillType.All, 3);
            
            //BaseSkill
            foreach (var baseSkill in _randomSkill.baseSkills)
            {
                GameObject element = Instantiate(m_skill_Element);
                TextMeshProUGUI element_text = element.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                if(baseSkill.Item1 != null)
                    element_text.text = baseSkill.Item1.name;
                else
                    element_text.text = baseSkill.Item2.name;

                element.transform.parent = group.transform;
            }
            
            //UpgradeSkill
            foreach (var upgradeSkill in _randomSkill.upgradeSkills)
            {
                GameObject element = Instantiate(m_skill_Element);
                TextMeshProUGUI element_text = element.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                if(upgradeSkill.Item1 != null)
                    element_text.text = upgradeSkill.Item1.name;
                else
                    element_text.text = upgradeSkill.Item2.name;

                element.transform.parent = group.transform;
            }

            GameObject preRandom = Instantiate(m_preRandom);
            preRandom.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            group.transform.parent = preRandom.transform;
            preRandom.transform.parent = m_viewport.transform;

            _scrollViewAnim.content = preRandom.transform.GetComponent<RectTransform>();
            _scrollViewAnim.enabled = true;
        }
    }
}