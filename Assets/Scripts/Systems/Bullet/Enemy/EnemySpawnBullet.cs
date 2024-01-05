using System;
using System.Collections.Generic;
using GDD.ObjectPool;
using UnityEngine;
using UnityEngine.Pool;

namespace GDD
{
    public class EnemySpawnBullet : BulletIgnition
    {
        private EnemyBulletObjectPool _bulletObjectPool;
        public EnemyBulletObjectPool bulletObjectPool
        {
            get => _bulletObjectPool;
        }

        public override void Start()
        {
            base.Start();
            
            _bulletObjectPool = gameObject.AddComponent<EnemyBulletObjectPool>();
            
        }

        public override List<GameObject> OnSpawnBullet(float distance, float power, int shot, float damage, BulletType type, BulletShotSurroundMode surroundMode, BulletShotMode shotMode, ObjectPoolBuilder builder = null)
        {
            return base.OnSpawnBullet(distance, power, shot, damage, type, surroundMode, shotMode, _bulletObjectPool);
        }
    }
}