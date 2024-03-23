using System;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class VFXBulletsTime : AutoDisableVFX
    {
        protected VFXObjectPool _vfxObjectPool;
        
        public VFXObjectPool vfxObjectPool
        {
            set => _vfxObjectPool = value;
        }
        
        protected override void OnDisableVFX()
        {
            _vfxObjectPool.ReturnToPool();
        }
    }
}