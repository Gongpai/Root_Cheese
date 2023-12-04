using System.Collections;
using System.Collections.Generic;
using GDD.Spatial_Partition;
using UnityEngine;

namespace GDD
{
    public class ProjectileReflectionBulletFireManeuver : BulletFireManeuver
    {
        private SpawnerProjectileReflectionBulletCalculate _spawnerPRBC;
        public override void Start()
        {
            base.Start();

            _spawnerPRBC = gameObject.AddComponent<SpawnerProjectileReflectionBulletCalculate>();
            _spawnerPRBC.shot = 3;
            _spawnerPRBC.Set_spawnPoint = _enemySpawnBullet.spawnPoint.gameObject;
            _spawnerPRBC.surroundMode = BulletShotSurroundMode.Front;
            _spawnerPRBC.OnStart();
        }
        
        public override void ToggleFire(EnemySpawnBullet enemySpawnBullet)
        {
            base.ToggleFire(enemySpawnBullet);
            
            enemySpawnBullet.bulletObjectPool.Set_GameObject = bullet;
            
            EnemyBulletConfig _enemyBulletConfig = m_enemyBulletConfig;
            enemySpawnBullet.OnSpawnBullet(
                m_bulletSpawnDistance,
                _enemyBulletConfig.bullet_power,
                3,
                _enemyBulletConfig.damage,
                BulletType.ProjectileReflection,
                BulletShotSurroundMode.Front,
                BulletShotMode.SurroundMode
            );
        }
    }
}