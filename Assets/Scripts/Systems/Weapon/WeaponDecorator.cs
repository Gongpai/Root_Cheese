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
        
        public float damage
        {
            get => _decoratedWeapon.damage * _weaponConfigStats.damage;
        }

        public float rate
        {
            get => _decoratedWeapon.rate * _weaponConfigStats.rate;
        }

        public int shot
        {
            get => _decoratedWeapon.shot;
        }
        
        public float power
        {
            get => _decoratedWeapon.power * _weaponConfigStats.power;
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
        
        public float shield { get => _attachment.shield * _attachmentStats.shield; }
        public float effect_health { get => _attachment.effect_health * _attachmentStats.effect_health; }
        public GameObject attachmentObject { get => _attachment.attachmentObject; }
        public float attachmentSpinSpeed { get => _attachment.attachmentSpinSpeed * _attachmentStats.attachmentSpinSpeed; }
        public float attachmentDamage { get => _attachment.attachmentDamage  * _attachmentStats.attachmentDamage; }
    }
}