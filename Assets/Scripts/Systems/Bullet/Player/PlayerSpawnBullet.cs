﻿using System;
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

        public override List<GameObject> OnSpawnBullet(float distance, float power, int shot, float damage,Transform target, BulletType type, BulletShotSurroundMode surroundMode, BulletShotMode shotMode, ObjectPoolBuilder builder = null)
        {
            return base.OnSpawnBullet(distance, power, shot, damage, target, type, surroundMode, shotMode, _bulletObjectPool);
        }
        
        public override void OnSpawnGrenade(int shot, float damge, int[] posIndex = default, ObjectPoolBuilder builder = null)
        {
            base.OnSpawnGrenade(shot, damge, posIndex, _bulletObjectPool);
        }
    }
}