using System;
using System.Collections;
using UnityEngine;

namespace GDD
{
    public class AutoFireWeaponDecorator : MonoBehaviour
    {
        private SpawnBullet _spawnBullet;
        private WeaponSystem _weaponSystem;
        private bool _isWeaponDecorated;

        private void Start()
        {
            _spawnBullet = GetComponent<SpawnBullet>();
            _weaponSystem = GetComponent<WeaponSystem>();

            StartCoroutine(AutoFire());
        }

        IEnumerator AutoFire()
        {
            while (true)
            {
               yield return new WaitForSeconds(1);
               _weaponSystem.ToggleFire(_spawnBullet);
            }
        }
    }
}