using System;
using UnityEngine;
using UnityEngine.Pool;

namespace GDD
{
    public class SpawnBullet : MonoBehaviour
    {
        [SerializeField] private GameObject m_spawnPoint;
        private BulletObjectPool _bulletObjectPool;

        public BulletObjectPool bulletObjectPool
        {
            get => _bulletObjectPool;
        }

        private void Start()
        {
            _bulletObjectPool = gameObject.AddComponent<BulletObjectPool>();
            _bulletObjectPool.spawnPoint = m_spawnPoint.transform;
        }

        public Bullet OnSpawnBullet()
        {
            return _bulletObjectPool.OnSpawn();
        }
    }
}