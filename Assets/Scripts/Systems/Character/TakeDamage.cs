using System;
using GDD.ObjectPool;
using UnityEngine;

namespace GDD
{
    public class TakeDamage : MonoBehaviour
    {
        private GameObjectPool _bullet;
        private GameManager GM;
        
        //Character Owner
        private Transform _ownerLayer;

        public Transform ownerLayer
        {
            get => _ownerLayer;
            set => _ownerLayer = value;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            GM = GameManager.Instance;
            _bullet = GetComponent<GameObjectPool>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            Transform layer = other.transform.parent;
            CharacterSystem _characterSystem;
            if (other.transform.parent == GM.enemies[0].transform.parent && ownerLayer.transform.parent == GM.players[0].transform.parent)
            {
                _characterSystem = other.gameObject.GetComponent<CharacterSystem>();
                OnTakeDamage(_characterSystem, _ownerLayer.GetComponent<PlayerBulletObjectPool>().weapon.damage);
            }
            else if (other.transform.parent == GM.players[0].transform.parent && ownerLayer.transform.parent == GM.enemies[0].transform.parent)
            {
                _characterSystem = other.gameObject.GetComponent<CharacterSystem>();
                OnTakeDamage(_characterSystem, 10);
            }
            else
            {
                Debug.LogError("Not Found HealthSystem Component");
            }

            if(other.tag != "Enemy" && other.tag == "Player")
                _bullet.ReturnToPool();
        }

        protected void OnTakeDamage(CharacterSystem characterSystem, float damage)
        {
            if (characterSystem.hp - damage > 0)
                characterSystem.hp -= damage;
            else
                characterSystem.hp = 0;
            
            print("Current Health : " + characterSystem.hp);
        }
    }
}