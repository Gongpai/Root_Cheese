using System;
using GDD.ObjectPool;
using UnityEngine;
using UnityEngine.Pool;

namespace GDD
{
    public class BulletObjectPool : ObjectPoolBuilder
    {
        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            base.Update();
        }

        public override GameObjectPool CreatePooledItem()
        {
            return base.CreatePooledItem();
        }

        public override void OnReturnToPool(GameObjectPool bullet)
        {
            Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            
            base.OnReturnToPool(bullet);
        }

        public override void OnTakeFromPool(GameObjectPool bullet)
        {
            base.OnTakeFromPool(bullet);
        }

        public override void OnDestroyPoolObject(GameObjectPool bullet)
        {
            base.OnDestroyPoolObject(bullet);
        }

        public override GameObject OnSpawn()
        {
            GameObject bullet_gobj = base.OnSpawn().gameObject;
            bullet_gobj.layer = LayerMask.NameToLayer("Bullet");
            TakeDamage bullet_TD;
            
            if (bullet_gobj.GetComponent<TakeDamage>() == null)
            {
                bullet_TD = bullet_gobj.gameObject.AddComponent<TakeDamage>();
            }
            else
            {
                bullet_TD = bullet_gobj.GetComponent<TakeDamage>();
            }
            bullet_TD.ownerLayer = transform;
            
            return bullet_gobj;
        }
    }
}