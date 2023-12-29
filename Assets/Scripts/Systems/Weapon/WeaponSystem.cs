using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace GDD
{
    public class WeaponSystem : MonoBehaviour
    {
        [Header("Config Weapon")]
        [SerializeField] private WeaponConfig m_weaponConfig;
        [SerializeField] private int m_weaponConfigPathIndex;
        [SerializeField] private WeaponAttachment m_mainAttachment;
        [SerializeField] private int m_mainAttachmentPathIndex;
        [SerializeField] private WeaponAttachment m_secondaryAttachment;
        [SerializeField] private int m_secondaryAttachmentPathIndex;
        private Tuple<WeaponConfig, int> _weaponConfig;
        private Tuple<WeaponAttachment, int> _mainAttachment;
        private Tuple<WeaponAttachment, int> _secondaryAttachment;

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
        
        public Tuple<WeaponConfig, int> weaponConfig
        {
            get => _weaponConfig;
            set => _weaponConfig = value;
        }

        public Tuple<WeaponAttachment, int> mainAttachment
        {
            get => _mainAttachment;
            set => _mainAttachment = value;
        }

        public Tuple<WeaponAttachment, int> secondaryAttachment
        {
            get => _secondaryAttachment;
            set => _secondaryAttachment = value;
        }
        
        public WeaponConfigStats weaponConfigStats
        {
            get => _weaponConfigStats;
            set => _weaponConfigStats = value;
        }

        public WeaponAttachmentStats attachmentStats
        {
            get => _attachmentStats;
            set => _attachmentStats = value;
        }
        
        private void Start()
        {
            _characterSystem = GetComponent<CharacterSystem>();
            _weaponConfigStats = new WeaponConfigStats();
            _attachmentStats = new WeaponAttachmentStats();
            
            if(!_characterSystem.enabled)
                return;
            
            _weaponConfig = new Tuple<WeaponConfig, int>(m_weaponConfig, m_weaponConfigPathIndex);
            _mainAttachment = new Tuple<WeaponAttachment, int>(m_mainAttachment, m_mainAttachmentPathIndex);
            _secondaryAttachment = new Tuple<WeaponAttachment, int>(m_secondaryAttachment, m_secondaryAttachmentPathIndex);
            SetWeaponConfig();
        }

        public void SetWeaponConfig()
        {
            _weapon = new WeaponDecorator(_weaponConfig.Item1, null, _weaponConfigStats, _attachmentStats);
        }
        
        public void ToggleFire(PlayerSpawnBullet playerSpawnBullet)
        {
            //print($"PlayerS is null : {playerSpawnBullet == null} | Weapon is null : {_weapon == null}");
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
        
        
        public void SetMainSkill(WeaponConfig weaponConfig, int index)
        {
            print("Index Set Skill : " + index);
            _weaponConfig = new Tuple<WeaponConfig, int>(weaponConfig, index);
            _weapon = new WeaponDecorator(_weaponConfig.Item1, null, _weaponConfigStats, _attachmentStats);
            Debug.Log(weapon.mainName + $" Player Name {gameObject.name} SETTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT");
            Decorate();
        }

        public void SetAttachment(WeaponAttachment weaponAttachment, int index)
        {
            if (_mainAttachment.Item1 == null || _secondaryAttachment.Item1 != null)
            {
                _mainAttachment = new Tuple<WeaponAttachment, int>(weaponAttachment, index);
                Debug.Log($"Main Attachment {weaponAttachment.attachmentName} | Player Name {gameObject.name}");
            }
            else
            {
                _secondaryAttachment = new Tuple<WeaponAttachment, int>(weaponAttachment, index);
                Debug.Log($"Secondary Attachment {weaponAttachment.attachmentName} | Player Name {gameObject.name}");
            }

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
            _weapon = new Weapon(_weaponConfig.Item1);
            _isDecorated = !_isDecorated;
        }

        public void Decorate()
        {
            if ((_mainAttachment.Item1 && !_secondaryAttachment.Item1))
                _weapon = new WeaponDecorator(_weapon, _mainAttachment.Item1, null, null);

            if ((_mainAttachment.Item1 && _secondaryAttachment.Item1))
                _weapon = new WeaponDecorator(new WeaponDecorator(_weapon, _mainAttachment.Item1, null, null), _secondaryAttachment.Item1, null, null);
            
            _isDecorated = !_isDecorated;
        }
    }
}