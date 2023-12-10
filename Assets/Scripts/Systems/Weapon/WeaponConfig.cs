﻿using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "NewWeaponConfig", menuName = "GDD/Weapon/Config", order = 1)]
    public class WeaponConfig : ScriptableObject, IWeapon
    {
        [Header("Name")]
        public string weaponName;
        
        [Header("Setting")]
        [SerializeField]
        private float m_damage;

        [Range(0, 5)] [Tooltip("Rate of firing per second")]
        [SerializeField]private float m_rate;

        [Range(1, 8)] [Tooltip("Shot Number")]
        [SerializeField]private int m_shot = 1;
        
        [SerializeField]
        private float m_power;
        
        [SerializeField]
        private float m_bullet_spawn_distance;
        
        [SerializeField]
        private BulletShotSurroundMode m_surroundMode;

        [SerializeField] 
        private BulletShotMode m_bulletShotMode = BulletShotMode.SurroundMode;

        public float damage
        {
            get => m_damage;
        }

        public float rate
        {
            get => m_rate;
        }

        public int shot
        {
            get => m_shot;
        }

        public float power
        {
            get => m_power;
        }

        public BulletShotSurroundMode surroundMode
        {
            get => m_surroundMode;
        }

        public BulletShotMode bulletShotMode
        {
            get => m_bulletShotMode;
        }
        
        public float bullet_spawn_distance
        {
            get => m_bullet_spawn_distance;
        }
        
        public float shield { get => 0; }
        public float effect_health { get => 0; }
        public GameObject attachmentObject { get => null; }
        public float attachmentSpinSpeed { get => 0; }
        public float attachmentDamage { get => 0; }
    }
}