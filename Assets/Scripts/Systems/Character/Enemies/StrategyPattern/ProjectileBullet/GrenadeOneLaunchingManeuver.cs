using GDD.Spatial_Partition;

namespace GDD
{
    public class GrenadeOneLaunchingManeuver : GrenadeLaunchManeuver
    {
        public override void OnFire(IPawn enemy)
        {
            base.OnFire(enemy);
            
            if (enemy != null)
            {
                print("On Maneuver");
                _coroutines.Add(StartCoroutine(Waiting(
                    () =>
                    {
                        OneLaunching();
                    },
                    m_enemyBulletConfig.timedelay)));
            }
        }
        
        public override void ToggleFire(EnemySpawnBullet enemySpawnBullet)
        {
            base.ToggleFire(enemySpawnBullet);
            
            enemySpawnBullet.bulletObjectPool.Set_GameObject = bullet;

            EnemyBulletConfig _enemyBulletConfig = m_enemyBulletConfig;
            enemySpawnBullet.OnSpawnBullet(
                m_bulletSpawnDistance,
                _enemyBulletConfig.bullet_power,
                1,
                _enemyBulletConfig.damage,
                BulletShotSurroundMode.Surround
            );
        }
    }
}