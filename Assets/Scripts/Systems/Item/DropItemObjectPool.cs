using System.Linq;
using GDD.ObjectPool;
using GDD.Spawner;
using UnityEngine;

namespace GDD
{
    public class DropItemObjectPool : SpawnerObjectsPool
    {
        [SerializeField] protected Transform m_spawnPoint;
        
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        public override void Start()
        {
            //Check ProjectileLauncherCalculate in Spawn Point
            Transform launcher = null;
            if(m_spawnPoint.childCount > 0) 
                launcher = m_spawnPoint.GetChild(0);
            if (m_spawnPoint.childCount <= 0 || launcher == null && launcher.name != "launcher")
            {
                _PLC = new GameObject("launcher").AddComponent<ProjectileLauncherCalculate>();
                _PLC.transform.parent = m_spawnPoint;
            }
            else
            {
                _PLC = launcher.GetComponent<ProjectileLauncherCalculate>();
            }
            
            base.Start();
        }

        public override GameObjectPool CreatePooledItem()
        {
            ItemObject _itemObject = Instantiate(m_object).AddComponent<ItemObject>();
            _itemObject.gameObject.layer = LayerMask.NameToLayer("EXP");
            _itemObject.objectPool = Pool;
            _itemObject.delay = 5.0f;
            return _itemObject;
        }

        protected override void OnCreateObjectLoop(GameObject gObject, int i)
        {
            base.OnCreateObjectLoop(gObject, i);

            if (i > m_spawnCount / 2 && GM.playMode == PlayMode.Multiplayer && GM.players.Count > 1)
            {
                gObject.GetComponent<ItemObject>().target = GM.players.Keys.ElementAt(1).transform;
            }
            else
            {
                gObject.GetComponent<ItemObject>().target = GM.players.Keys.ElementAt(0).transform;
            }
        }

        protected override Vector3 GetTargetPosition(Vector3 pos, int i)
        {
            Vector3 targetPos = pos;
            if (i > (m_spawnCount / 2) - 1)
            {
                targetPos *= -1;
            } 
            return targetPos;
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