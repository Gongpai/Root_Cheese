﻿using System;
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
        private Coroutine _coroutinereturnpool;
        
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

            _coroutinereturnpool = StartCoroutine(WaitReturnToPool(120));
        }

        IEnumerator WaitReturnToPool(float time)
        {
            yield return new WaitForSeconds(time);
            ReturnToPool();
        }

        private void OnTriggerEnter(Collider other)
        {
            /*
            if(_ownerLayer.parent == GM.enemy_layer)
                print("Take Damage : " + other.name);
            */
            
            Transform layer = other.transform.parent;
            CharacterSystem _characterSystem;
            //print($"Other Layer : {layer == null} | GM E Layer : {GM.enemy_layer == null} | OwnerLayer : {ownerLayer.transform.parent} | GM P Layer : {GM.player_layer}");
            if (layer == GM.enemy_layer && ownerLayer.transform.parent == GM.player_layer)
            {
                //print("Enemy Take Damage");
                _characterSystem = other.gameObject.GetComponent<CharacterSystem>();
                OnTakeDamage(_characterSystem, _damage);
                ReturnToPool();
            }
            else if (layer == GM.player_layer && ownerLayer.transform.parent == GM.enemy_layer)
            {
                //print("Character Take Damage");
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
            if (!_is_undying)
            {
                //print("Return TO Poooooollllll!!!!!!!!!!!!");
                _bullet.ReturnToPool();
                StopCoroutine(_coroutinereturnpool);
            }
        }
        
        public void ReturnBulletToPool()
        {
            _bullet.ReturnToPool();
        }

        protected void OnTakeDamage(CharacterSystem characterSystem, float damage)
        {
            if (characterSystem.GetShield() > 0)
            {
                if (characterSystem.GetShield() - damage > 0)
                    characterSystem.SetShiel(characterSystem.GetShield() - damage);
                else
                    characterSystem.SetShiel(0);
            }
            else
            {
                if (characterSystem.GetHP() - damage > 0)
                    characterSystem.SetHP(characterSystem.GetHP() - damage);
                else
                    characterSystem.SetHP(0);
            }

            //print("Current Health : " + characterSystem.hp);
        }
    }
}