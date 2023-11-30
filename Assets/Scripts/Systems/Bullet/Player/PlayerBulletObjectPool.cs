using GDD.ObjectPool;
using UnityEngine;

namespace GDD
{
    public class PlayerBulletObjectPool : BulletObjectPool
    {
        private IWeapon _weapon;

        public IWeapon weapon
        {
            get => _weapon;
            set => _weapon = value;
        }

        public override GameObjectPool CreatePooledItem()
        {
            GameObjectPool bullet = Instantiate(_gameObjectPool, GM.Get_Bullet_Pool.transform).AddComponent<PlayerBullet>();
            bullet.objectPool = Pool;

            return bullet;
        }

        public override GameObject OnSpawn()
        {
            GameObject bullet_gobj = base.OnSpawn();
            PlayerBullet playerBullet = bullet_gobj.GetComponent<PlayerBullet>();
            //print("Bullet Pos : " + bullet.transform.position + " || Spawn Pos : " + _spawnPoint.position);
            playerBullet._weapon = _weapon;
            
            return bullet_gobj;
        }
    }
}