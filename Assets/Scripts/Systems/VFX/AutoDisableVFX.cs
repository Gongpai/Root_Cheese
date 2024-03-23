using System;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class AutoDisableVFX : MonoBehaviour
    {
        [SerializeField] protected List<ParticleSystem> _particleSystems;
        protected ParticleSystem _particleSystem;
        
        public List<ParticleSystem> particleSystems
        {
            get => _particleSystems;
        }

        protected virtual void Start()
        {
            float currentDuration = 0;
            foreach (var particleSystem in _particleSystems)
            {
                if (particleSystem.duration > currentDuration)
                {
                    currentDuration = particleSystem.duration;
                    _particleSystem = particleSystem;
                }
            }
        }

        protected virtual void Update()
        {
            if(_particleSystem.isStopped)
                OnDisableVFX();
        }

        protected virtual void OnDisableVFX()
        {
            gameObject.SetActive(false);
        }
    }
}