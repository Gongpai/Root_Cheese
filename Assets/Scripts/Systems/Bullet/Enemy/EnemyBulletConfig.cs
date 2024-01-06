using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "NewEnemyBulletConfig", menuName = "GDD/Enemy/EnemyBulletConfig", order = 1)]
    public class EnemyBulletConfig : ScriptableObject
    {
        [Header("BulletConfig")]
        [Range(0, 100)]
        [SerializeField]
        private float m_damage;
        
        [Range(0, 100)] [Tooltip("Rate of firing per second")]
        [SerializeField]private float m_rate = 0.25f;

        [SerializeField] private float m_bullet_power = 5;
        
        [SerializeField]private float m_timedelay = 0.25f;

        [SerializeField] private int m_shot = 1;

        [SerializeField] private BulletType m_bulletType;
        
        public float damage
        {
            get => m_damage;
        }
        
        public float rate
        {
            get => m_rate;
        }
        
        public float bullet_power
        {
            get => m_bullet_power;
        }
        
        public float timedelay
        {
            get => m_timedelay;
        }

        public int shot
        {
            get => m_shot;
        }

        public BulletType bulletType
        {
            get => m_bulletType;
        }
    }
}