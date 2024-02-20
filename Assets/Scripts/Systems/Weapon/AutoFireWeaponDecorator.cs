using System;
using System.Collections;
using UnityEngine;

namespace GDD
{
    public class AutoFireWeaponDecorator : MonoBehaviour
    {
        private PlayerSpawnBullet _playerSpawnBullet;
        private WeaponSystem _weaponSystem;
        private bool _isWeaponDecorated;

        private void Start()
        {
            _playerSpawnBullet = GetComponent<PlayerSpawnBullet>();
            _weaponSystem = GetComponent<WeaponSystem>();

            StartCoroutine(AutoFire());
        }

        IEnumerator AutoFire()
        {
            while (true)
            {
               yield return new WaitForSeconds(1);
               //_weaponSystem.ToggleFire(_playerSpawnBullet);
            }
        }
    }
}