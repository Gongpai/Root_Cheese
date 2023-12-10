using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "NewAttachmentSkillUpgrade", menuName = "GDD/SkillUpgrade/Attachment", order = 1)]
    public class AttachmentSkillUpgrade : ScriptableObject
    {
        [SerializeField]
        private float _shield = 1;
        [SerializeField]
        private float _effect_health = 1;
        [SerializeField]
        private float _attachmentSpinSpeed = 1;
        [SerializeField]
        private float _attachmentDamage = 1;
        
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