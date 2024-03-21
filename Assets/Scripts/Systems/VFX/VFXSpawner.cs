using GDD.ObjectPool;
using UnityEngine;

namespace GDD
{
    public class VFXSpawner : ObjectPoolBuilder
    {
        private Transform _parent;
        
        public override GameObjectPool CreatePooledItem()
        {
            GameObjectPool _vfxObject = Instantiate(_gameObjectPool).AddComponent<VFXObjectPool>();

            if (_parent == null)
            {
                _parent = new GameObject("VFX Group").transform;
                _parent.transform.position = Vector3.zero;
            }

            _vfxObject.transform.SetParent(_parent);
            _vfxObject.objectPool = Pool;
            _vfxObject.GetComponent<VFXBulletsTime>().vfxObjectPool = (VFXObjectPool)_vfxObject;
            return _vfxObject;
        }
    }
}