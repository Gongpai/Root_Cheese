using GDD.Spatial_Partition;

namespace GDD
{
    public class GrenadeKeepLaunchingManeuver : GrenadeLaunchManeuver
    {
        public override void Start()
        {
            base.Start();
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
                BulletType.Projectile,
                BulletShotSurroundMode.Surround,
                BulletShotMode.SurroundMode
            );
        }
    }
}