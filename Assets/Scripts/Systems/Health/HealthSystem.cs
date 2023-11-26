using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GDD
{
    public abstract class HealthSystem : MonoBehaviour
    {
        [SerializeField] protected Slider m_hp_bar;
        [SerializeField] protected float m_hp = 100;

        public float hp
        {
            get => m_hp;
            set => m_hp = value;
        }

        public virtual void Update()
        {
            m_hp_bar.value = m_hp / 100;
        }
    }
}