using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "NewMainSkillUpgrade", menuName = "GDD/SkillUpgrade/Main", order = 0)]
    public class MainSkillUpgrade : ScriptableObject
    {
        [SerializeField]
        private float _damage = 1;
        [SerializeField]
        private float _rate = 1;
        [SerializeField]
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