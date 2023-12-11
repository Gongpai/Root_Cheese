using System;
using Unity.VisualScripting;
using UnityEngine;

namespace GDD
{
    public class WeaponDecorator : IWeapon
    {
        private readonly IWeapon _decoratedWeapon;
        private readonly WeaponAttachment _attachment;
        private readonly WeaponConfigStats _weaponConfigStats;
        private readonly WeaponAttachmentStats _attachmentStats;

        public WeaponDecorator(IWeapon weapon, WeaponAttachment attachment, WeaponConfigStats weaponConfigStats, WeaponAttachmentStats attachmentStats)
        {
            _attachment = attachment;
            _decoratedWeapon = weapon;
            _weaponConfigStats = weaponConfigStats;
            _attachmentStats = attachmentStats;
        }

        public string mainName { get => _decoratedWeapon.mainName; }
        public string mainAttachmentName { get => "AttMain"; }
        public string secAttachmentName { get => _attachment.attachmentName; }

        public GameObject bulletObject
        {
            get => _decoratedWeapon.bulletObject;
        }
        public virtual float damage
        {
            get
            {
                if (_weaponConfigStats == null)
                    return _decoratedWeapon.damage * 1;
                else
                    return _decoratedWeapon.damage * _weaponConfigStats.damage;
            }
        }

        public virtual float rate
        {
            get
            {
                if (_weaponConfigStats == null)
                    return _decoratedWeapon.rate * 1;
                else
                    return _decoratedWeapon.rate * _weaponConfigStats.rate;
            }
        }

        public int shot
        {
            get => _decoratedWeapon.shot;
        }
        
        public float power
        {
            get
            {
                if (_weaponConfigStats == null)
                    return _decoratedWeapon.power * 1;
                else
                    return _decoratedWeapon.power * _weaponConfigStats.power;
            }
        }
        
        public float bullet_spawn_distance
        {
            get => _decoratedWeapon.bullet_spawn_distance;
        }

        public BulletShotSurroundMode surroundMode
        {
            get => _decoratedWeapon.surroundMode;
        }
        
        public BulletShotMode bulletShotMode
        {
            get => _decoratedWeapon.bulletShotMode;
        }
        
        public float shield {
            get
            {
                if (_attachment == null)
                    return _decoratedWeapon.shield;
                else if (_attachmentStats == null)
                    return _attachment.shield * 1;
                else
                    return _attachment.shield * _attachmentStats.shield;
            }
        }

        public virtual float effect_health
        {
            get
            {
                if (_attachment == null)
                    return _decoratedWeapon.effect_health;
                else if (_attachmentStats == null)
                    return _attachment.effect_health * 1;
                else
                    return _attachment.effect_health * _attachmentStats.effect_health;
            }
        }

        public GameObject attachmentObject
        {
            get
            {
                if (_attachment == null)
                    return _decoratedWeapon.attachmentObject;
                else
                    return _attachment.attachmentObject;
            }
        }

        public virtual float attachmentSpinSpeed
        {
            get
            {
                if (_attachment == null)
                    return _decoratedWeapon.attachmentSpinSpeed;
                else if (_attachmentStats == null)
                    return _attachment.attachmentSpinSpeed * 1;
                else
                    return _attachment.attachmentSpinSpeed * _attachmentStats.attachmentSpinSpeed;
            }
        }

        public virtual float attachmentDamage
        {
            get
            {
                if (_attachment == null)
                    return _decoratedWeapon.attachmentDamage;
                else if (_attachmentStats == null)
                    return _attachment.attachmentDamage * 1;
                else
                    return _attachment.attachmentDamage * _attachmentStats.attachmentDamage; 
            }
        }
    }
}