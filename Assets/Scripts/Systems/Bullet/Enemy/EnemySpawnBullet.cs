using System;
using UnityEngine;
using UnityEngine.Pool;

namespace GDD
{
    public class EnemySpawnBullet : MonoBehaviour
    {
        [SerializeField] private GameObject m_spawnPoint;
        private EnemyBulletObjectPool _bulletObjectPool;

        public BulletObjectPool bulletObjectPool
        {
            get => _bulletObjectPool;
        }

        private void Start()
        {
            _bulletObjectPool = gameObject.AddComponent<EnemyBulletObjectPool>();
            _bulletObjectPool.spawnPoint = m_spawnPoint.transform;
        }

        public EnemyBullet OnSpawnBullet()
        {
            return _bulletObjectPool.OnSpawn().GetComponent<EnemyBullet>();
        }
    }
}