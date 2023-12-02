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

        [SerializeField] private BulletShotSurroundMode m_shotSurroundMode;
        [Range(1, 8)]
        [SerializeField] private int m_shot = 1;
        
        [SerializeField]private float m_timedelay = 0.25f;

        [SerializeField] private float m_bullet_spawn_distance;
        
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

        public BulletShotSurroundMode shotSurroundMode
        {
            get => m_shotSurroundMode;
        }

        public int shot
        {
            get => m_shot;
        }
        
        public float timedelay
        {
            get => m_timedelay;
        }

        public float bullet_spawn_distance
        {
            get => m_bullet_spawn_distance;
        }
    }
}