using GDD.ObjectPool;
using UnityEngine;

namespace GDD
{
    public class CharacterBullet : GameObjectPool
    {
       protected VFXSpawner _vfxSpawner;

        public VFXSpawner vfxSpawner
        {
            set => _vfxSpawner = value;
        }

        public override void ReturnToPool()
        {
            print("Return TO Poooooollllll!!!!!!!!!!!!");
            if (_vfxSpawner != null && !_vfxSpawner.isSpawnObjectNull)
            {
                GameObject vfxObject = _vfxSpawner.OnSpawn();
                vfxObject.transform.position = transform.position;
            }
            
            base.ReturnToPool();
        }
    }
}