using UnityEngine;

namespace GDD
{
    public class WeaponAttachmentStats : MonoBehaviour
    {
        private float _shield = 0;
        private float _effect_health = 0;
        private float _attachmentSpinSpeed = 0;
        private float _attachmentDamage = 0;
        
        public float shield 
        {
            get => _shield;
            set => _shield = value;
        }
        public float effect_health 
        { 
            get => _effect_health;
            set => _effect_health = value;
        }

        public float attachmentSpinSpeed
        {
            get => _attachmentSpinSpeed;
            set => _attachmentSpinSpeed = value;
        }

        public float attachmentDamage
        {
            get => _attachmentDamage;
            set => _attachmentDamage = value;
        }
    }
}