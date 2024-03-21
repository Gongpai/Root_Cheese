using System;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class VFXBulletsTime : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> _particleSystems;
        private ParticleSystem _particleSystem;
        private VFXObjectPool _vfxObjectPool;

        public List<ParticleSystem> particleSystems
        {
            get => _particleSystems;
        }

        public VFXObjectPool vfxObjectPool
        {
            set => _vfxObjectPool = value;
        }

        private void Start()
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

        private void Update()
        {
            if(_particleSystem.isStopped)
                _vfxObjectPool.ReturnToPool();
        }
    }
}