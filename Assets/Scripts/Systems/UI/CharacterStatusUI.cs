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
        [SerializeField] private Slider m_ShieldProgress;
        
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
                Destroy(gameObject);

            string hpText = "";
            if (_characterSystem.GetShield() <= 0)
                hpText = $"HP : {_characterSystem.GetHP()} / {_characterSystem.GetMaxHP()}";
            else
                hpText = $"HP : {_characterSystem.GetHP()} / {_characterSystem.GetMaxHP()} | Shield : {_characterSystem.GetShield()} / {_characterSystem.GetMaxShield()}";
                    
            m_textHP.text = hpText;
            m_HPProgress.fillAmount = _characterSystem.GetHP() / _characterSystem.GetMaxHP();
            
            if(m_ShieldProgress != null)
                m_ShieldProgress.value = _characterSystem.GetShield() / _characterSystem.GetMaxShield();
            
            m_textEXPLevel.text = $"Level : {_characterSystem.GetLevel()} | EXP : {_characterSystem.GetUpdateEXP()} / {_characterSystem.GetMaxEXP()}";
            m_EXPProgress.fillAmount = (float)_characterSystem.GetUpdateEXP() / (float)_characterSystem.GetMaxEXP();
        }
    }
}