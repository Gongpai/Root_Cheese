using System;
using GDD.Spatial_Partition;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GDD
{
    public abstract class CharacterSystem : Pawn
    {
        [SerializeField] private TextMeshProUGUI m_hp_text;
        [SerializeField] protected Slider m_hp_bar;
        [SerializeField] protected float m_hp = 100;

        public float hp
        {
            get => m_hp;
            set => m_hp = value;
        }

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

        public override Transform GetPawnTransform()
        {
            return transform;
        }

        public virtual void OnDestroy()
        {
            
        }
    }
}