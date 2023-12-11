using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "NewAttachmentSkillUpgrade", menuName = "GDD/SkillUpgrade/Attachment", order = 1)]
    public class AttachmentSkillUpgrade : ScriptableObject
    {
        [Header("Skill Info")]
        [SerializeField] 
        private Sprite _skillIcon;
        [SerializeField] 
        private string _skillName;
        [SerializeField] 
        private string _skillDescription;

        [Header("Skill Upgrade")] [SerializeField]
        private float _maxHealth = 0;
        [SerializeField]
        private float _shield = 1;
        [SerializeField]
        private float _effect_health = 1;
        [SerializeField]
        private float _attachmentSpinSpeed = 1;
        [SerializeField]
        private float _attachmentDamage = 1;

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

        public float maxHealth
        {
            get => _maxHealth;
        }
        
        public float shield 
        {
            get => _shield;
        }
        public float effect_health 
        { 
            get => _effect_health;
        }

        public float attachmentSpinSpeed
        {
            get => _attachmentSpinSpeed;
        }

        public float attachmentDamage
        {
            get => _attachmentDamage;
        }
    }
}