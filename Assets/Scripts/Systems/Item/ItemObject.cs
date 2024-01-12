using System;
using System.Timers;
using GDD.ObjectPool;
using UnityEngine;

namespace GDD
{
    public class ItemObject : GameObjectPool
    {
        [SerializeField]
        private float _currentTime = 2.0f;
        private float _delay;

        public float delay
        {
            set => _delay = value;
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
            }
            else
            {
                ReturnToPool();
            }
        }

        public override void ReturnToPool()
        {
            base.ReturnToPool();
        }
    }
}