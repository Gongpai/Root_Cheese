using GDD.ObjectPool;
using UnityEngine;

namespace GDD
{
    public class CharacterBullet : GameObjectPool
    {
        protected VFXSpawner _vfxSpawner;
        protected GameObject _vfxObject;

        public VFXSpawner vfxSpawner
        {
            set => _vfxSpawner = value;
        }

        public GameObject vfxObject
        {
            get => _vfxObject;
        }

        public override void ReturnToPool()
        {
            print("Return TO Poooooollllll!!!!!!!!!!!!");
            if (_vfxSpawner != null && !_vfxSpawner.isSpawnObjectNull)
            {
                _vfxObject = _vfxSpawner.OnSpawn();
                _vfxObject.transform.position = transform.position;
            }

            base.ReturnToPool();
        }
    }
}