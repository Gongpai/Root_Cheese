using System;
using System.Collections;
using GDD.ObjectPool;
using GDD.PUN;
using Photon.Pun;
using UnityEngine;

namespace GDD
{
    public class TakeDamage : MonoBehaviourPun
    {
        private GameObjectPool _bullet;
        private GameManager GM;
        private float _damage;
        private bool _is_undying = false;
        private Coroutine _coroutinereturnpool;
        public int OwnerViewID = -1;
        
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
            OwnerViewID = GM.playerClient.GetComponent<MonoBehaviourPun>().photonView.ViewID;
            
            _coroutinereturnpool = StartCoroutine(WaitReturnToPool(120));
        }

        IEnumerator WaitReturnToPool(float time)
        {
            yield return new WaitForSeconds(time);
            ReturnToPool();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (ownerLayer.transform.parent == GM.enemy_layer)
            {
                print($"Layer Check {GM.player_layer.name}");
                print($"Layer A : {other.transform.parent.name} || B : {ownerLayer.transform.parent.name}");
            }

            Transform layer = other.transform.parent;
            PunCharacterHealth _punCharacterHealth;
            bool has_punCharacterHealth = other.TryGetComponent<PunCharacterHealth>(out _punCharacterHealth);

            if (!has_punCharacterHealth)
            {
                print($"{other.transform.name} : Pun is null!!!!!!!!!!!!!!!!!!");
                return;
            }

            if (layer == GM.enemy_layer && ownerLayer.transform.parent == GM.player_layer)
            {
                print("Enemy Take Damage");
                CharacterSystem _characterSystem = other.gameObject.GetComponent<CharacterSystem>();
                OnTakeDamage(_characterSystem, _damage);
                ReturnToPool();
            }
            else if (layer == GM.player_layer && ownerLayer.transform.parent == GM.enemy_layer)
            {
                print("Character Take Damage");
                _punCharacterHealth.TakeDamage(_damage, OwnerViewID); 
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
                    characterSystem.SetShield(characterSystem.GetShield() - damage);
                else
                    characterSystem.SetShield(0);
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