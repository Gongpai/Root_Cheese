using System.Collections;
using GDD.Spatial_Partition;
using UnityEngine;

namespace GDD
{
    public class GrenadeLaunchManeuver : BulletFireManeuver
    {
        public virtual void OnFire(IPawn enemy)
        {
            
        }
        
        public virtual void ToggleFire(EnemySpawnBullet enemySpawnBullet)
        {
            
        }

        protected Coroutine KeepLaunching(float time)
        {
            return StartCoroutine(Firing(time));
        }
        
        protected void OneLaunching()
        {
            ToggleFire(_enemySpawnBullet);
        }
    }
}