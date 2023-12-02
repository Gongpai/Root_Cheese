using GDD.ObjectPool;
using UnityEngine;

namespace GDD
{
    public class EnemyBulletObjectPool : BulletObjectPool
    {
        public override GameObjectPool CreatePooledItem()
        {
            GameObjectPool bullet = Instantiate(_gameObjectPool, GM.Get_Bullet_Pool.transform).AddComponent<EnemyBullet>();
            bullet.objectPool = Pool;

            return bullet;
        }

        public override GameObject OnSpawn()
        {
            GameObject bullet_gobj = base.OnSpawn();
            bullet_gobj.GetComponent<TakeDamage>().damage = GetComponent<EnemySystem>()._enemyBulletConfig.damage;
            
            return bullet_gobj;
        }
    }
}