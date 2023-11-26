using System;
using UnityEngine;
using UnityEngine.Pool;

namespace GDD
{
    public class Bullet : MonoBehaviour
    {
        public IObjectPool<Bullet> objectPool { get; set; }
        public IWeapon _weapon { get; set; }

        private void OnDisable()
        {
            
        }

        public void ReturnToPool()
        {
            if(gameObject.activeSelf)
                objectPool.Release(this);
        }

        public void ResetBullet(Transform spawnPoint)
        {
            transform.position = spawnPoint.position;
        }
    }
}