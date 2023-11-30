using UnityEngine;
using UnityEngine.Pool;

namespace GDD.ObjectPool
{
    public class GameObjectPool : MonoBehaviour
    {
        public IObjectPool<GameObjectPool> objectPool { get; set; }

        public virtual void OnDisable()
        {
            
        }

        public virtual void ReturnToPool()
        {
            if(gameObject.activeSelf)
                objectPool.Release(this);
        }

        public virtual void ResetBullet(Transform spawnPoint)
        {
            transform.position = spawnPoint.position;
        }
    }
}