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
        private CharacterSystem _characterSystem;
        private IWeapon _weapon;
        private bool _isFiring;
        private bool _isDecorated;

        public IWeapon weapon
        {
            get => _weapon;
        }
        
        public WeaponConfig weaponConfig
        {
            get => _weaponConfig;
        }

        public WeaponAttachment mainAttachment
        {
            get => _mainAttachment;
        }

        public WeaponAttachment secondaryAttachment
        {
            get => _secondaryAttachment;
        }
        
        public WeaponConfigStats weaponConfigStats
        {
            get => _weaponConfigStats;
        }

        public WeaponAttachmentStats attachmentStats
        {
            get => _attachmentStats;
        }

        private void Start()
        {
            _characterSystem = GetComponent<CharacterSystem>();
            _weaponConfigStats = gameObject.AddComponent<WeaponConfigStats>();
            _attachmentStats = gameObject.AddComponent<WeaponAttachmentStats>();
            _weapon = new WeaponDecorator(_weaponConfig, null, _weaponConfigStats, _attachmentStats);
        }

        public void ToggleFire(PlayerSpawnBullet playerSpawnBullet)
        {
            playerSpawnBullet.bulletObjectPool.weapon = _weapon;
            playerSpawnBullet.bulletObjectPool.Set_GameObject = _weapon.bulletObject;
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
        
        
        public void Set_MainSkill(WeaponConfig weaponConfig)
        {
            Debug.Log(weapon.mainName + " SETTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT");
            _weaponConfig = weaponConfig;
            _weapon = new WeaponDecorator(_weaponConfig, null, _weaponConfigStats, _attachmentStats);
            Decorate();
        }

        public void Set_Attachment(WeaponAttachment weaponAttachment)
        {
            Debug.Log(weapon.mainName + " SETTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT");
            if (_mainAttachment == null || _secondaryAttachment != null)
                _mainAttachment = weaponAttachment;
            else
                _secondaryAttachment = weaponAttachment;
            Decorate();
        }

        public void UpgradeMainSkill(MainSkillUpgrade _upgrade)
        {
            if(_upgrade.damage > 0)
                _weaponConfigStats.damage += _upgrade.damage;
            
            if(_upgrade.power > 0)
                _weaponConfigStats.power += _upgrade.power;
            
            if (_upgrade.rate > 0)
                _weaponConfigStats.rate -= _upgrade.rate;
        }
        
        public void UpgradeAttachmentSkill(AttachmentSkillUpgrade _upgrade)
        {
            if(_upgrade.maxHealth > 0)
                _characterSystem.SetMaxHP(_characterSystem.GetMaxHP() * _upgrade.maxHealth);
            
            if(_upgrade.shield > 0)
                _attachmentStats.shield += _upgrade.shield;
            
            if(_upgrade.effect_health > 0)
                _attachmentStats.effect_health += _upgrade.effect_health;
            
            if(_upgrade.attachmentSpinSpeed > 0)
                _attachmentStats.attachmentSpinSpeed += _upgrade.attachmentSpinSpeed;
            
            if(_upgrade.attachmentDamage > 0)
                _attachmentStats.attachmentDamage += _upgrade.attachmentDamage;
        }

        public void Reset()
        {
            _weapon = new Weapon(_weaponConfig);
            _isDecorated = !_isDecorated;
        }

        public void Decorate()
        {
            if ((_mainAttachment && !_secondaryAttachment))
                _weapon = new WeaponDecorator(_weapon, _mainAttachment, null, null);

            if ((_mainAttachment && _secondaryAttachment))
                _weapon = new WeaponDecorator(new WeaponDecorator(_weapon, _mainAttachment, null, null), _secondaryAttachment, null, null);
            
            _isDecorated = !_isDecorated;
        }
    }
}