﻿using System;
using System.Collections;
using GDD.ObjectPool;
using GDD.PUN;
using GDD.Timer;
using Photon.Pun;
using UnityEngine;

namespace GDD
{
    public class TakeDamage : MonoBehaviourPun, ITakeDamage
    {
        private CharacterBullet _bullet;
        private GameManager GM;
        [SerializeField] private float _damage;
        private bool m_is_undying = false;
        private bool is_MasterClient = true;
        private Coroutine _coroutinereturnpool;
        public int OwnerViewID = -1;
        
        //Character Owner
        [SerializeField] private Transform _ownerLayer;

        public Transform ownerLayer
        {
            get => _ownerLayer;
            set => _ownerLayer = value;
        }

        public float damage
        {
            get => _damage;
            set => _damage = value;
        }

        public bool is_undying
        {
            set => m_is_undying = value;
        }

        public CharacterBullet bullet
        {
            get => _bullet;
            set => _bullet = value;
        }

        protected virtual void Awake()
        {
            if(_bullet == null)
                _bullet = GetComponent<CharacterBullet>();
            
            if(!m_is_undying)
                _coroutinereturnpool = StartCoroutine(WaitReturnToPool(5));
        }

        protected virtual void OnEnable()
        {
            GM = GameManager.Instance;
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            OwnerViewID = GM.playerMasterClient.GetComponent<MonoBehaviourPun>().photonView.ViewID;
        }
        
        protected virtual void Update()
        {
            
        }

        IEnumerator WaitReturnToPool(float time)
        {
            yield return new WaitForSeconds(time);
            ReturnToPool();
        }
        
        protected virtual void OnTriggerEnter(Collider other)
        {
            OnTakeDamage(other);
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            
        }

        public virtual int GetViewID()
        {
            return OwnerViewID;
        }

        public virtual void SetViewID(int OwnerViewID)
        {
            this.OwnerViewID = OwnerViewID;
        }

        protected virtual void OnDisable()
        {
            
        }

        protected virtual void OnTakeDamage(Collider other)
        {
            /*
            if (ownerLayer != null || ownerLayer.transform.parent == GM.enemy_layer)
            {
                print($"OwnerLayer : {ownerLayer.name}");
                print($"P/E Layer Check {GM.player_layer.name}");
                print($"Layer A : {other.transform.parent.name} || B : {ownerLayer.transform.parent.name}");
            }*/
            
            //Get PunCharacterHealth
            PunCharacterHealth _punCharacterHealth;
            bool has_punCharacterHealth = other.TryGetComponent<PunCharacterHealth>(out _punCharacterHealth);

            //Get Layer Target
            Transform layer = other.transform.parent;
                
            
            /*
            print($"Hit Object Layer Null = {layer == null}");
            print($"Owner Layer Null = {ownerLayer == null}");
            print($"GM = {GM.name}");
            
            if(GM.enemy_layer == null)
                print($"Enemy Layer Null = {GM.enemy_layer == null}");
            
            print($"Player Layer Null = {GM.player_layer == null}");
            */
            
            //Check PunCharacterHealth
            if (has_punCharacterHealth)
            {
                CharacterSystem _characterSystem = other.GetComponent<CharacterSystem>();
                
                //Check Client
                is_MasterClient = _punCharacterHealth.CharacterSystem.isMasterClient;
                if (!is_MasterClient)
                {
                    return;
                }

                //Check Enemy Layer & Set HP / Shield
                if (layer == GM.enemy_layer && ownerLayer.transform.parent == GM.player_layer)
                {
                    //print("Enemy Take Damage");
                    _punCharacterHealth.TakeDamage(_damage);
                    _characterSystem.PlayHPBarAttack();
                    ReturnToPool();
                }
                //Check Player Layer & Set HP / Shield
                else if (layer == GM.player_layer && ownerLayer.transform.parent == GM.enemy_layer)
                {
                    //print("Character Take Damage");
                    _punCharacterHealth.TakeDamage(_damage);
                    _characterSystem.PlayHPBarAttack();
                    ReturnToPool();
                }
                else
                {
                    //Debug.LogError("Not Found HealthSystem Component");
                }
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle") || other.gameObject.layer == LayerMask.NameToLayer("Floor"))
                ReturnToPool();
        }

        public virtual void ReturnToPool()
        {
            if(_bullet == null)
                return;
            
            if (!m_is_undying)
            {
                //print("Return TO Poooooollllll!!!!!!!!!!!!");
                _bullet.ReturnToPool();
                StopCoroutine(_coroutinereturnpool);
            }
        }
        
        public virtual void ReturnBulletToPool()
        {
            if(_bullet == null)
                return;
            
            _bullet.ReturnToPool();
        }
    }
}