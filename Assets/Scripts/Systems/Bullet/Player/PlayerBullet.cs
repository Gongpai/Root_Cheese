using System;
using GDD.ObjectPool;
using UnityEngine;
using UnityEngine.Pool;

namespace GDD
{
    public class PlayerBullet : GameObjectPool
    {
        public IWeapon _weapon { get; set; }

        public override void OnDisable()
        {
            
        }
    }
}