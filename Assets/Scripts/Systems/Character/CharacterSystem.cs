using System;
using GDD.Spatial_Partition;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GDD
{
    public abstract class CharacterSystem : Pawn, ICharacter
    {
        [Header("Player Stats Setting")] 
        [SerializeField] protected TextMeshProUGUI m_hp_text;
        [SerializeField] protected Slider m_hp_bar;
        [SerializeField] protected Slider m_shield_bar;
        [SerializeField] protected float m_hp = 100;
        [SerializeField] protected float m_max_HP = 100;
        [SerializeField] protected float m_shield = 100;
        [SerializeField] protected int _maxEXP = 100;
        [SerializeField] protected float m_levelUp = 1.1f;
        protected bool _isMasterClient = true;
        private int _EXP;
        private int _level;

        public bool isMasterClient
        {
            get => _isMasterClient;
            set => _isMasterClient = value;
        }

        public virtual void OnEnable()
        {
            
        }
        
        public virtual void Start()
        {
            
        }

        public virtual void Update()
        {
            m_hp_bar.value = m_hp / m_max_HP;
            m_hp_text.text = $"HP : {m_hp} / {m_max_HP} || Shield : {GetShield()} / {GetMaxShield()}";

            if (m_shield_bar != null && (GetShield() / GetMaxShield()) > 0)
                m_shield_bar.value = GetShield() / GetMaxShield();
            else
                m_shield_bar.value = 0;
                    
            if(m_hp <= 0)
                OnCharacterDead();

            LevelProgress();
        }

        protected void LevelProgress()
        {
            if (_EXP >= _maxEXP)
            {
                _level++;
                _maxEXP = (int)(_maxEXP * m_levelUp);
                _EXP = 0;
            }
        }

        public virtual void OnCharacterDead()
        {
            gameObject.SetActive(false);
        }
        
        public virtual void OnDisable()
        {
            
        }

        public virtual float GetHP()
        {
            return m_hp;
        }

        public virtual void SetMaxHP(float maxHP)
        {
            m_max_HP = maxHP;
        }

        public virtual float GetMaxHP()
        {
            return m_max_HP;
        }

        public virtual void SetHP(float hp)
        {
            if (hp >= m_max_HP)
                hp = m_max_HP;
            else
                m_hp = hp;
        }

        public virtual float GetMaxShield()
        {
            return 0;
        }

        public virtual float GetShield()
        {
            return m_shield;
        }

        public virtual void SetShield(float shield)
        {
            if (shield >= GetMaxShield())
                shield = GetMaxShield();
            else
                m_shield = shield;
        }

        public void SetMaxEXP(int maxEXP)
        {
            _maxEXP = maxEXP;
        }

        public int GetMaxEXP()
        {
            return _maxEXP;
        }

        public void SetEXP(int EXP)
        {
            _EXP = EXP;
        }

        public int GetEXP()
        {
            return _EXP;
        }

        public int GetLevel()
        {
            return _level;
        }

        public override Transform GetPawnTransform()
        {
            return transform;
        }

        public virtual void OnDestroy()
        {
            
        }
    }
}