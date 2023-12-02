using System;
using System.Collections;
using UnityEngine;

namespace GDD
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] private WeaponConfig _weaponConfig;
        [SerializeField] private WeaponAttachment mainAttachment;
        [SerializeField] private WeaponAttachment secondaryAttachment;

        private bool _isFiring;
        private IWeapon _weapon;
        private bool _isDecorated;

        public IWeapon Get_Weapon
        {
            get => _weapon;
        }

        private void Start()
        {
            _weapon = new Weapon(_weaponConfig);
        }

        public void ToggleFire(PlayerSpawnBullet playerSpawnBullet)
        {
            if (secondaryAttachment == null)
            {
                playerSpawnBullet.bulletObjectPool.Set_GameObject = mainAttachment.m_bullet_prefab;
            }
            else
            {
                playerSpawnBullet.bulletObjectPool.Set_GameObject = secondaryAttachment.m_bullet_prefab;
            }

            playerSpawnBullet.bulletObjectPool.weapon = _weapon;
            playerSpawnBullet.OnSpawnBullet(
                _weapon.bullet_spawn_distance,
                _weapon.power,
                _weapon.shot,
                _weapon.damage,
                _weapon.surroundMode
                );
        }

        public void Reset()
        {
            _weapon = new Weapon(_weaponConfig);
            _isDecorated = !_isDecorated;
        }

        public void Decorate()
        {
            if ((mainAttachment && !secondaryAttachment))
                _weapon = new WeaponDecorator(_weapon, mainAttachment);

            if ((mainAttachment && !secondaryAttachment))
                _weapon = new WeaponDecorator(new WeaponDecorator(_weapon, mainAttachment), secondaryAttachment);
        }
    }
}