using UnityEngine;

namespace GDD
{
    public class WeaponConfigStats : MonoBehaviour
    {
        private float _damage = 1;
        private float _rate = 1;
        private float _power = 1;

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