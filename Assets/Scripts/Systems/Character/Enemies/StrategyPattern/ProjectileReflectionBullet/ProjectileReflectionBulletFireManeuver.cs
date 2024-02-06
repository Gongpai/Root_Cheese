using System.Collections;
using System.Collections.Generic;
using GDD.Spatial_Partition;
using UnityEngine;

namespace GDD
{
    public class ProjectileReflectionBulletFireManeuver : BulletFireManeuver
    {
        private SpawnerProjectileReflectionBulletCalculate _spawnerPRBC;

        public SpawnerProjectileReflectionBulletCalculate spawnerPRBC
        {
            get => _spawnerPRBC;
        }
        
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
            _punECC.OnProjectileReflectionLinesEnable(true);
        }

        public override void Maneuver(EnemyState pawn, Transform target)
        {
            _punECC.OnProjectileReflectionLinesEnable(true);
            OnShowProjectileReflectionLines();
            
            base.Maneuver(pawn, target);
        }

        public override void Truce()
        {
            base.Truce();

            _enemyAttackState.isLockRot = false;
        }

        protected override IEnumerator Firing(float time)
        {
            _punECC.OnProjectileReflectionLinesEnable(false);
            OnHideProjectileReflectionLines();
            
            return base.Firing(time);
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
                _target,
                _enemyBulletConfig.bulletType,
                BulletShotSurroundMode.Front,
                BulletShotMode.SurroundMode
            );
        }

        public void OnHideProjectileReflectionLines()
        {
            if (_enemyAttackState == null)
                _enemyAttackState = GetComponent<EnemyAttackState>();
            
            _enemyAttackState.isLockRot = true;
            if (_spawnerPRBC != null)
            {
                foreach (var PRBC in _spawnerPRBC.PRBCs)
                {
                    PRBC.OnStop();
                }
            }
        }

        public void OnShowProjectileReflectionLines()
        {
            if (_enemyAttackState == null)
                _enemyAttackState = GetComponent<EnemyAttackState>();
            
            _enemyAttackState.isLockRot = false;
            if (_spawnerPRBC != null)
            {
                foreach (var PRBC in _spawnerPRBC.PRBCs)
                {
                    PRBC.OnStart();
                }
            }
        }
    }
}