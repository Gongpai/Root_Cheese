using System;
using System.Collections.Generic;
using GDD.ObjectPool;
using UnityEngine;
using UnityEngine.Pool;

namespace GDD
{
    public class PlayerSpawnBullet : BulletIgnition
    {
        private PlayerBulletObjectPool _bulletObjectPool;
        public PlayerBulletObjectPool bulletObjectPool
        {
            get => _bulletObjectPool;
        }

        public override void Start()
        {
            base.Start();
            
            _bulletObjectPool = gameObject.AddComponent<PlayerBulletObjectPool>();
        }

        public override List<GameObject> OnSpawnBullet(float distance, float power, int shot, BulletShotSurroundMode surroundMode, ObjectPoolBuilder builder = null)
        {
           return base.OnSpawnBullet(distance, power, shot, surroundMode,  _bulletObjectPool);
        }
    }
}