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
        [SerializeField] private TextMeshProUGUI m_hp_text;
        [SerializeField] protected Slider m_hp_bar;
        [SerializeField] protected float m_hp = 100;
        [SerializeField] protected float m_shield = 0;

        public virtual void OnEnable()
        {
            
        }
        
        public virtual void Start()
        {
            
        }

        public virtual void Update()
        {
            m_hp_bar.value = m_hp / 100;
            m_hp_text.text = "HP : " + m_hp +" / " + 100;
            
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