using UnityEngine;
using UnityEngine.Pool;

namespace GDD.ObjectPool
{
    public class ObjectPoolBuilder : MonoBehaviour
    {
        [SerializeField]protected int _maxPoolSize = 10;
        [SerializeField]protected int _stackDefaultCapacity = 5;

        protected GameManager GM;
        protected IObjectPool<GameObjectPool> _pool;
        protected GameObject _gameObjectPool;

        public int maxPoolSize
        {
            get => _maxPoolSize;
            set => _maxPoolSize = value;
        }

        public int stackDefaultCapacity
        {
            get => _stackDefaultCapacity;
            set => _stackDefaultCapacity = value;
        }

        public GameObject Set_GameObject
        {
            set => _gameObjectPool = value;
        }
        
        public IObjectPool<GameObjectPool> Pool
        {
            get
            {
                if (_pool == null)
                    _pool = new ObjectPool<GameObjectPool>(CreatePooledItem,
                        OnTakeFromPool,
                        OnReturnToPool,
                        OnDestroyPoolObject,
                        true,
                        stackDefaultCapacity,
                        maxPoolSize);

                return _pool;
            }
        }
        
        public virtual void Start()
        {
            GM = GameManager.Instance;
        }

        public virtual void Update()
        {
            
        }

        public virtual GameObjectPool CreatePooledItem()
        {
            return null;
        }

        public virtual void OnReturnToPool(GameObjectPool gObject)
        {
            gObject.gameObject.SetActive(false);
        }
        
        public virtual void OnTakeFromPool(GameObjectPool bullet)
        {
            bullet.gameObject.SetActive(true);
        }

        public virtual void OnDestroyPoolObject(GameObjectPool bullet)
        {
            Destroy(bullet.gameObject);
        }
        
        public virtual GameObject OnSpawn()
        {
            GameObject bullet = Pool.Get().gameObject;
            return bullet;
        }
    }
}