using System;
using System.Timers;
using GDD.ObjectPool;
using UnityEngine;

namespace GDD
{
    [RequireComponent(typeof(Rigidbody))]
    public class ItemObject : GameObjectPool
    {
        [SerializeField]
        private float _currentTime = 2.0f;
        private float _currentTransitionTime = 0f;
        private float _delay;
        private Transform _target = null;
        Vector3 _itemPos;
        private Rigidbody _rig;

        public Transform target
        {
            set => _target = value;
        }

        public float delay
        {
            set
            {
                _delay = value;
                _currentTime = value;
            }
        }

        private void Awake()
        {
            _rig = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _currentTime = _delay;
        }

        private void Update()
        {
            if (_currentTime > 0)
            {
                _currentTime -= Time.deltaTime;
                _itemPos = transform.position;
            }
            else
            {
                OnCollect();
            }
        }

        protected void OnCollect()
        {
            if (_currentTransitionTime < 1.0f)
            {
                _rig.useGravity = false;
                _currentTransitionTime += Time.deltaTime;
                print($"{_currentTransitionTime} | Transitionnnnnnnnnnnnnnnnnn");
                print($"Item Pos : {_itemPos} | Target Pos {_target.position}");

                Vector3 PosLerp = Vector3.Lerp(_itemPos, _target.position + new Vector3(0, 1, 0), _currentTransitionTime);

                if (_target != null)
                    _rig.position = PosLerp;
                
                print($"Pos Lerp {PosLerp}");
            }
            else if(_currentTransitionTime >= 1.0f)
            {
                ReturnToPool();
            }
        }
        
        public override void ReturnToPool()
        {
            //print($"Return to pool : |{gameObject.name}|");
            _currentTransitionTime = 0;
            base.ReturnToPool();
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }
    }
}