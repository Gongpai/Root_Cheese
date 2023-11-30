using System;
using UnityEngine;
using UnityEngine.Pool;

namespace GDD
{
    public class PlayerSpawnBullet : MonoBehaviour
    {
        [SerializeField] private GameObject m_spawnPoint;
        private PlayerBulletObjectPool _bulletObjectPool;

        public PlayerBulletObjectPool bulletObjectPool
        {
            get => _bulletObjectPool;
        }

        private void Start()
        {
            _bulletObjectPool = gameObject.AddComponent<PlayerBulletObjectPool>();
            _bulletObjectPool.spawnPoint = m_spawnPoint.transform;
        }

        public PlayerBullet OnSpawnBullet()
        {
            return _bulletObjectPool.OnSpawn().GetComponent<PlayerBullet>();
        }
    }
}