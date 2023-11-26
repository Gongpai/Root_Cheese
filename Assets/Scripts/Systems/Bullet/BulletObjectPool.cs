using System;
using UnityEngine;
using UnityEngine.Pool;

namespace GDD
{
    public class BulletObjectPool : MonoBehaviour
    {
        [SerializeField]private int _maxPoolSize = 10;
        [SerializeField]private int _stackDefaultCapacity = 5;

        private GameManager GM;
        
        private IObjectPool<Bullet> _pool;
        private GameObject _bulletGameObject;
        private Transform _spawnPoint;
        private IWeapon _weapon;
        private Vector3 spawnPos;

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

        public GameObject Set_BulletGameObject
        {
            set => _bulletGameObject = value;
        }

        public Transform spawnPoint
        {
            set => _spawnPoint = value;
        }
        
        public IObjectPool<Bullet> Pool
        {
            get
            {
                if (_pool == null)
                    _pool = new ObjectPool<Bullet>(CreatePooledItem,
                        OnTakeFromPool,
                        OnReturnToPool,
                        OnDestroyPoolObject,
                        true,
                        stackDefaultCapacity,
                        maxPoolSize);

                return _pool;
            }
        }

        public IWeapon weapon
        {
            set => _weapon = value;
        }
        
        private void Start()
        {
            GM = GameManager.Instance;
        }

        private void Update()
        {
            spawnPos = _spawnPoint.position;
        }

        private Bullet CreatePooledItem()
        {
            Bullet bullet = Instantiate(_bulletGameObject, GM.Get_Bullet_Pool.transform).AddComponent<Bullet>();
            bullet.objectPool = Pool;

            return bullet;
        }

        private void OnReturnToPool(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
        }
        
        private void OnTakeFromPool(Bullet bullet)
        {
            bullet.gameObject.SetActive(true);
        }

        private void OnDestroyPoolObject(Bullet bullet)
        {
            Destroy(bullet.gameObject);
        }
        
        public Bullet OnSpawn()
        {
            Bullet bullet = Pool.Get();
            bullet.transform.position = spawnPos;
            print("Bullet Pos : " + bullet.transform.position + " || Spawn Pos : " + _spawnPoint.position);
            bullet._weapon = _weapon;

            TakeDamage bullet_TD;
            if (bullet.GetComponent<TakeDamage>() == null)
            {
                bullet_TD = bullet.gameObject.AddComponent<TakeDamage>();
            }
            else
            {
                bullet_TD = bullet.GetComponent<TakeDamage>();
            }
            bullet_TD._weapon = _weapon;
            bullet_TD.ownerLayer = transform;
            
            Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(_spawnPoint.forward * 10, ForceMode.Impulse);
            
            return bullet;
        }
    }
}