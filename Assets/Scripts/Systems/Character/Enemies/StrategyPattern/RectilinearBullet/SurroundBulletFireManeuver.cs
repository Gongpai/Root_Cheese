namespace GDD
{
    public class SurroundBulletFireManeuver : BulletFireManeuver
    {
        public override void ToggleFire(EnemySpawnBullet enemySpawnBullet)
        {
            base.ToggleFire(enemySpawnBullet);
            
            enemySpawnBullet.bulletObjectPool.Set_GameObject = bullet;

            EnemyBulletConfig _enemyBulletConfig = m_enemyBulletConfig;
            enemySpawnBullet.OnSpawnBullet(
                m_bulletSpawnDistance,
                _enemyBulletConfig.bullet_power,
                8,
                _enemyBulletConfig.damage,
                BulletShotSurroundMode.Surround,
                BulletShotMode.SurroundMode
            );
        }
    }
}