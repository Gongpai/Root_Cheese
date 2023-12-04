using System;
using System.Collections;
using GDD.ObjectPool;
using UnityEngine;

namespace GDD
{
    public class TakeDamage : MonoBehaviour
    {
        private GameObjectPool _bullet;
        private GameManager GM;
        private float _damage;
        private bool _is_undying = false;
        
        //Character Owner
        private Transform _ownerLayer;

        public Transform ownerLayer
        {
            get => _ownerLayer;
            set => _ownerLayer = value;
        }

        public float damage
        {
            set => _damage = value;
        }

        public bool is_undying
        {
            set => _is_undying = value;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            GM = GameManager.Instance;
            _bullet = GetComponent<GameObjectPool>();

            WaitReturnToPool(10);
        }

        IEnumerator WaitReturnToPool(float time)
        {
            yield return new WaitForSeconds(time);
            ReturnToPool();
        }

        private void OnTriggerEnter(Collider other)
        {
            Transform layer = other.transform.parent;
            CharacterSystem _characterSystem;
            if (layer == GM.enemy_layer && ownerLayer.transform.parent == GM.player_layer)
            {
                _characterSystem = other.gameObject.GetComponent<CharacterSystem>();
                OnTakeDamage(_characterSystem, _damage);
                ReturnToPool();
            }
            else if (layer == GM.player_layer && ownerLayer.transform.parent == GM.enemy_layer)
            {
                _characterSystem = other.gameObject.GetComponent<CharacterSystem>();
                OnTakeDamage(_characterSystem, _damage);
                ReturnToPool();
            }
            else
            {
                //Debug.LogError("Not Found HealthSystem Component");
            }

            if(other.tag != "Enemy" && other.tag != "Player" && other.transform.parent != transform.parent)
                ReturnToPool();
        }

        private void ReturnToPool()
        {
            if(!_is_undying)
                _bullet.ReturnToPool();
        }
        
        public void ReturnBulletToPool()
        {
            _bullet.ReturnToPool();
        }

        protected void OnTakeDamage(CharacterSystem characterSystem, float damage)
        {
            if (characterSystem.hp - damage > 0)
                characterSystem.hp -= damage;
            else
                characterSystem.hp = 0;
            
            //print("Current Health : " + characterSystem.hp);
        }
    }
}