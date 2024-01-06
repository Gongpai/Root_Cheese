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

            if (GetComponent<SpawnerProjectileReflectionBulletCalculate>() == null)
            {
                _spawnerPRBC = gameObject.AddComponent<SpawnerProjectileReflectionBulletCalculate>();
                _spawnerPRBC.shot = m_enemyBulletConfig.shot;
                _spawnerPRBC.Set_spawnPoint = _enemySpawnBullet.spawnPoint.gameObject;
                _spawnerPRBC.surroundMode = BulletShotSurroundMode.Front;
                _spawnerPRBC.OnStart();
            }
        }

        public override void Maneuver(EnemyState pawn)
        {
            if (_spawnerPRBC != null)
            {
                foreach (var PRBC in _spawnerPRBC.PRBCs)
                {
                    PRBC.OnStart();
                }
            }
            
            base.Maneuver(pawn);
        }

        public override void Truce()
        {
            base.Truce();
            
            if (_spawnerPRBC != null)
            {
                foreach (var PRBC in _spawnerPRBC.PRBCs)
                {
                    PRBC.OnStop();
                }
            }
        }

        public override void ToggleFire(EnemySpawnBullet enemySpawnBullet, int[] posIndex = default)
        {
            base.ToggleFire(enemySpawnBullet, posIndex);
            
            enemySpawnBullet.bulletObjectPool.Set_GameObject = bullet;
            
            EnemyBulletConfig _enemyBulletConfig = m_enemyBulletConfig;
            enemySpawnBullet.OnSpawnBullet(
                m_bulletSpawnDistance,
                _enemyBulletConfig.bullet_power,
                _enemyBulletConfig.shot,
                _enemyBulletConfig.damage,
                _enemyBulletConfig.bulletType,
                BulletShotSurroundMode.Front,
                BulletShotMode.SurroundMode
            );
        }
    }
}