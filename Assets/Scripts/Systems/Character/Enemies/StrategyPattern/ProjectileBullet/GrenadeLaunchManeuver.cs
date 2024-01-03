using System.Collections;
using GDD.Spatial_Partition;
using UnityEngine;

namespace GDD
{
    public class GrenadeLaunchManeuver : BulletFireManeuver
    {
        public override void Start()
        {
            base.Start();
        }
        
        public override void OnFire(IPawn enemy)
        {
            base.OnFire(enemy);
        }
        
        public override void ToggleFire(EnemySpawnBullet enemySpawnBullet)
        {
            base.ToggleFire(enemySpawnBullet);
            
            print("Fire!!!!!!!!!!!");
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