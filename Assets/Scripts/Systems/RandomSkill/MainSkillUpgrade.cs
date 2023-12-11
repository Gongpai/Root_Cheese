using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "NewMainSkillUpgrade", menuName = "GDD/SkillUpgrade/Main", order = 0)]
    public class MainSkillUpgrade : ScriptableObject
    {
        [Header("Skill Info")]
        [SerializeField] 
        private Sprite _skillIcon;
        [SerializeField] 
        private string _skillName;
        [SerializeField] 
        private string _skillDescription;
        
        [Header("Skill Config")]
        [SerializeField]
        private float _damage = 1;
        [SerializeField]
        private float _rate = 1;
        [SerializeField]
        private float _power = 1;

        public Sprite skillIcon
        {
            get => _skillIcon;
        }
        
        public string skillName
        {
            get => _skillName;
        }

        public string skillDescription
        {
            get => _skillDescription;
        }
        
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