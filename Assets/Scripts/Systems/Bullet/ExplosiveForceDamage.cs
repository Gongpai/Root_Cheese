using System;
using UnityEngine;

namespace GDD
{
    public class ExplosiveForceDamage : MonoBehaviour
    {
        [SerializeField] private Vector3 m_ColliderCenter;
        [SerializeField] private float m_ColliderRadius;
        [SerializeField] private bool m_isTigger;
        [SerializeField] private float m_damageTimeCount = 0.5f;
        private GameObject _subObject;
        private SphereCollider _sphereCollider;
        private CharacterBullet _characterBullet;
        private TakeExplosiveDamage _takeExplosiveDamage;
        private TakeDamage _bulletTakeDamage;

        private void OnEnable()
        {
            if (_takeExplosiveDamage != null)
                _takeExplosiveDamage.enabled = false;
        }

        private void Update()
        {
            
        }

        /*private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, m_ColliderRadius);
        }*/

        private void OnDisable()
        {
            if (_subObject == null)
            {
                //Get TakeDamage Component
                _bulletTakeDamage = GetComponent<TakeDamage>();
                
                //Create New GameObject
                _subObject = new GameObject("Explosive Force Damage");
                
                //Add & Set TakeExplosiveDamage Component
                _takeExplosiveDamage = _subObject.AddComponent<TakeExplosiveDamage>();
                _takeExplosiveDamage.bullet = _bulletTakeDamage.bullet;
                _takeExplosiveDamage.timeCount = m_damageTimeCount;
                _takeExplosiveDamage.ownerLayer = _bulletTakeDamage.ownerLayer;
                _takeExplosiveDamage.OwnerViewID = _bulletTakeDamage.OwnerViewID;
                _takeExplosiveDamage.damage = _bulletTakeDamage.damage;
                
                //Add & Set SphereCollider Component
                _sphereCollider = _subObject.AddComponent<SphereCollider>();
                _sphereCollider.center = m_ColliderCenter;
                _sphereCollider.radius = m_ColliderRadius;
                _sphereCollider.isTrigger = m_isTigger;
            }

            //Get CharacterBullet Component
            if (_characterBullet == null)
            {
                _characterBullet = GetComponent<CharacterBullet>();
                
                if (_characterBullet == null)
                    return;
            }
            
            print($"Explosive!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            //Check Has SubObject
            GameObject _currentSubObject = _characterBullet.vfxObject.transform.GetChild(0).gameObject;
            TakeExplosiveDamage _takeExplosive = _currentSubObject.GetComponent<TakeExplosiveDamage>();
            if (_takeExplosive != null)
            {
                _subObject = _currentSubObject;
                _sphereCollider = _currentSubObject.GetComponent<SphereCollider>();
                _takeExplosiveDamage = _takeExplosive;
            }

            if (_currentSubObject != _subObject)
            {
                _subObject.transform.SetParent(_characterBullet.vfxObject.transform);
                _subObject.transform.localPosition = Vector3.zero;
                _subObject.transform.SetAsFirstSibling();
            }

            _subObject.transform.localScale = Vector3.one;
            _takeExplosiveDamage.enabled = true;
        }
    }
}