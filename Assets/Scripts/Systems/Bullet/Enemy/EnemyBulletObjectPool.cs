using GDD.ObjectPool;

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
    }
}