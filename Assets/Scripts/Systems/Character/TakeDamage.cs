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
        private bool m_is_undying = false;
        private bool is_MasterClient = true;
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
            set => m_is_undying = value;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            GM = GameManager.Instance;
            _bullet = GetComponent<GameObjectPool>();
            OwnerViewID = GM.playerMasterClient.GetComponent<MonoBehaviourPun>().photonView.ViewID;
            
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
            if (ownerLayer != null && ownerLayer.transform.parent == GM.enemy_layer)
            {
                print($"Layer Check {GM.player_layer.name}");
                print($"Layer A : {other.transform.parent.name} || B : {ownerLayer.transform.parent.name}");
            }
            */
            
            //Get PunCharacterHealth
            PunCharacterHealth _punCharacterHealth;
            bool has_punCharacterHealth = other.TryGetComponent<PunCharacterHealth>(out _punCharacterHealth);

            //Get Layer Target
            Transform layer = other.transform.parent;

            //Check PunCharacterHealth
            if (!has_punCharacterHealth)
            {
                print($"{other.transform.name} : Pun is null!!!!!!!!!!!!!!!!!!");
                return;
            }
            
            //Check Client
            is_MasterClient = _punCharacterHealth.CharacterSystem.isMasterClient;
            if(!is_MasterClient)
                return;

            //Check Enemy Layer & Set HP / Shield
            if (layer == GM.enemy_layer && ownerLayer.transform.parent == GM.player_layer)
            {
                print("Enemy Take Damage");
                _punCharacterHealth.TakeDamage(_damage, OwnerViewID); 
                ReturnToPool();
            }
            //Check Player Layer & Set HP / Shield
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

            //If Not Player And Enemy Tag Return To Pool Only
            if(other.tag != "Enemy" && other.tag != "Player" && other.transform.parent != transform.parent)
                ReturnToPool();
        }

        private void ReturnToPool()
        {
            if (!m_is_undying)
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
    }
}