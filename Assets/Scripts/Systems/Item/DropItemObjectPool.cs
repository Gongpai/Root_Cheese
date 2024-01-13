using GDD.ObjectPool;
using GDD.Spawner;
using UnityEngine;

namespace GDD
{
    public class DropItemObjectPool : SpawnerObjectsPool
    {
        public override void Start()
        {
            base.Start();
        }

        public override GameObjectPool CreatePooledItem()
        {
            ItemObject _itemObject = Instantiate(m_object).AddComponent<ItemObject>();
            _itemObject.gameObject.layer = LayerMask.NameToLayer("EXP");
            _itemObject.objectPool = Pool;
            _itemObject.delay = 10.0f;
            return _itemObject;
        }

        public override void OnTakeFromPool(GameObjectPool gObject)
        {
            GameObjectPool ggObject = gObject;
            
            base.OnTakeFromPool(ggObject);
        }

        public override void OnCreateObject()
        {
            base.OnCreateObject();
        }

        public override GameObject OnSpawn()
        {

            return base.OnSpawn();
        }
    }
}