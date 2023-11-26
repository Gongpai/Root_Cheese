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

        private void Start()
        {
            _weapon = new Weapon(_weaponConfig);
        }

        public void ToggleFire(SpawnBullet spawnBullet)
        {
            _isFiring = !_isFiring;

            if (_isFiring)
            {
                if (secondaryAttachment == null)
                {
                    spawnBullet.bulletObjectPool.Set_BulletGameObject = mainAttachment.m_bullet_prefab;
                }
                else
                {
                    spawnBullet.bulletObjectPool.Set_BulletGameObject = secondaryAttachment.m_bullet_prefab;
                }

                spawnBullet.bulletObjectPool.weapon = _weapon;
                spawnBullet.OnSpawnBullet();
            }
        }

        /*
        IEnumerator Fireweapon()
        {
            float firing_rate = 1.0f / _weapon.rate;

            while (_isFiring)
            {
                yield return new WaitForSeconds(firing_rate);
                Debug.Log("Fire!!!!!!!!!!!!!!");
            }
        }*/

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