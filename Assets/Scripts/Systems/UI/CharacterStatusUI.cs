using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class CharacterStatusUI : MonoBehaviour
    {
        [Header("HP")] 
        [SerializeField] private TextMeshProUGUI m_textHP;
        [SerializeField] private Image m_HPProgress;
        
        [Header("EXP And Level")] 
        [SerializeField] private TextMeshProUGUI m_textEXPLevel;
        [SerializeField] private Image m_EXPProgress;
        
        private CharacterSystem _characterSystem;

        public CharacterSystem characterSystem
        {
            get => _characterSystem;
            set => _characterSystem = value;
        }

        private void Update()
        {
            if(_characterSystem == null)
                return;

            m_textHP.text = $"HP : {_characterSystem.GetHP()}";
            m_HPProgress.fillAmount = _characterSystem.GetHP() / _characterSystem.GetMaxHP();
            m_textEXPLevel.text = $"Level : {_characterSystem.GetLevel()} | EXP : {_characterSystem.GetUpdateEXP()} / {_characterSystem.GetMaxEXP()}";
            m_EXPProgress.fillAmount = (float)_characterSystem.GetUpdateEXP() / (float)_characterSystem.GetMaxEXP();
        }
    }
}