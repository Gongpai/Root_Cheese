using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "NewWeaponAttachment", menuName = "GDD/Weapon/Attachment", order = 1)]
    public class WeaponAttachment : ScriptableObject
    {
        [Header("Name")]
        public string attachmentName;

        [Header("Attachment")] 
        [SerializeField]
        private float _shield;
        [SerializeField]
        private float _effect_health;
        [SerializeField]
        private GameObject _attachmentObject;
        [SerializeField]
        private float _attachmentSpinSpeed;
        [SerializeField]
        private float _attachmentDamage;
        
        public float shield { get => _shield; }
        public float effect_health { get => _effect_health; }
        public GameObject attachmentObject { get => _attachmentObject; }
        public float attachmentSpinSpeed { get => _attachmentSpinSpeed; }
        public float attachmentDamage { get => _attachmentDamage; }
    }
}