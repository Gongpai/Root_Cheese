using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "NewWeaponAttachment", menuName = "GDD/Weapon/Attachment", order = 1)]
    public class WeaponAttachment : ScriptableObject
    {
        [Header("Name")]
        public string attachmentName;

        [Header("Bullet Object")] 
        public GameObject m_bullet_prefab;
        
        [Header("Setting")]
        [Range(0, 60)][Tooltip("Increase damage")]
        [SerializeField]
        private float m_damage;

        [Range(0, 60)] [Tooltip("Increase rate of firing per second")]
        [SerializeField]private float m_rate;

        [Range(1, 8)] [Tooltip("Increase shot Number")]
        [SerializeField]private int m_shot = 1;

        [SerializeField] private float m_power;
        
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
        
        public GameObject bullet_prefab
        {
            get => m_bullet_prefab;
        }
    }
}