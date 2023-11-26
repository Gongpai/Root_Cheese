using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "NewWeaponConfig", menuName = "GDD/Weapon/Config", order = 1)]
    public class WeaponConfig : ScriptableObject, IWeapon
    {
        [Header("Name")]
        public string weaponName;
        
        [Header("Setting")]
        [Range(0, 60)]
        [SerializeField]
        private float m_damage;

        [Range(0, 60)] [Tooltip("Rate of firing per second")]
        [SerializeField]private float m_rate;

        [Range(0, 60)] [Tooltip("Shot Number")]
        [SerializeField]private int m_shot;

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
    }
}