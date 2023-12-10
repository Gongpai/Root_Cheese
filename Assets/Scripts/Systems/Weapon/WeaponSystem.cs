using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace GDD
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] private WeaponConfig _weaponConfig;
        [SerializeField] private WeaponAttachment _mainAttachment;
        [SerializeField] private WeaponAttachment _secondaryAttachment;

        private WeaponConfigStats _weaponConfigStats;
        private WeaponAttachmentStats _attachmentStats;
        private IWeapon _weapon;
        private bool _isFiring;
        private bool _isDecorated;

        public WeaponConfigStats weaponConfigStats
        {
            get => _weaponConfigStats;
        }

        public WeaponAttachmentStats attachmentStats
        {
            get => _attachmentStats;
        }

        public IWeapon Get_Weapon
        {
            get => _weapon;
        }

        private void Start()
        {
            _weapon = new Weapon(_weaponConfig);
            _weaponConfigStats = gameObject.AddComponent<WeaponConfigStats>();
            _attachmentStats = gameObject.AddComponent<WeaponAttachmentStats>();
        }

        public void ToggleFire(PlayerSpawnBullet playerSpawnBullet)
        {
            playerSpawnBullet.bulletObjectPool.weapon = _weapon;
            playerSpawnBullet.OnSpawnBullet(
                _weapon.bullet_spawn_distance,
                _weapon.power,
                _weapon.shot,
                _weapon.damage,
                BulletType.Rectilinear,
                _weapon.surroundMode,
                _weapon.bulletShotMode
                );
        }
        
        
        public void Set_WeaponConfig(WeaponConfig weaponConfig)
        {
            _weaponConfig = weaponConfig;
        }

        public void mainAttachment(WeaponAttachment weaponAttachment)
        {
            _mainAttachment = weaponAttachment;
            Decorate();
        }

        public void secondaryAttachment(WeaponAttachment weaponAttachment)
        {
            _secondaryAttachment = weaponAttachment;
            Decorate();
        }

        public void Reset()
        {
            _weapon = new Weapon(_weaponConfig);
            _isDecorated = !_isDecorated;
        }

        public void Decorate()
        {
            if ((_mainAttachment && !_secondaryAttachment))
                _weapon = new WeaponDecorator(_weapon, _mainAttachment, _weaponConfigStats, _attachmentStats);

            if ((_mainAttachment && _secondaryAttachment))
                _weapon = new WeaponDecorator(new WeaponDecorator(_weapon, _mainAttachment, _weaponConfigStats, _attachmentStats), _secondaryAttachment, _weaponConfigStats, _attachmentStats);
            
            _isDecorated = !_isDecorated;
        }
    }
}