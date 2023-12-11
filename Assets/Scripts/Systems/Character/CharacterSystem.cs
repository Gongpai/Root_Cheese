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
        [SerializeField] private TextMeshProUGUI m_hp_text;
        [SerializeField] protected Slider m_hp_bar;
        [SerializeField] protected float m_hp = 100;
        [SerializeField] private float m_max_HP = 100;
        [SerializeField] protected float m_shield = 0;

        public virtual void OnEnable()
        {
            
        }
        
        public virtual void Start()
        {
            
        }

        public virtual void Update()
        {
            m_hp_bar.value = m_hp / m_max_HP;
            m_hp_text.text = "HP : " + m_hp +" / " + m_max_HP;
            
            if(m_hp <= 0)
                Destroy(gameObject);
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
            m_hp = hp;
        }

        public virtual float GetShield()
        {
            return m_shield;
        }

        public virtual void SetShiel(float shield)
        {
            m_shield = shield;
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