using GDD.ObjectPool;
using UnityEngine;

namespace GDD
{
    public class EnemyBulletObjectPool : BulletObjectPool
    {
        public override GameObjectPool CreatePooledItem()
        {
            CharacterBullet bullet = Instantiate(_gameObjectPool, GM.Get_Bullet_Pool.transform).AddComponent<EnemyBullet>();
            bullet.objectPool = Pool;

            return bullet;
        }

        public override GameObject OnSpawn()
        {
            return base.OnSpawn();
        }
    }
}