using System;
using UnityEngine;

namespace GDD
{
    public class RandomSkill : MonoBehaviour
    {
        private WeaponSystem _weaponSystem;
        private WeaponAttachment _weaponAttachment;
        
        private void Start()
        {
            _weaponSystem = GetComponent<WeaponSystem>();
            
            _weaponAttachment = Resources.Load<WeaponAttachment>("Presets/Player/SimplesecondaryWeaponAttachment");
            
            print("Weapon : " + _weaponAttachment.attachmentName);
        }

        private void ApplySkill()
        {
            
        }
    }
}