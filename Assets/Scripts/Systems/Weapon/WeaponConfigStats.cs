using UnityEngine;

namespace GDD
{
    public class WeaponConfigStats : MonoBehaviour
    {
        private float _damage = 0;
        private float _rate = 0;
        private float _power = 0;

        public float damage
        {
            get => _damage;
            set => _damage = value;
        }

        public float rate
        {
            get => _rate;
            set => _rate = value;
        }

        public float power
        {
            get => _power;
            set => _power = value;
        }
    }
}