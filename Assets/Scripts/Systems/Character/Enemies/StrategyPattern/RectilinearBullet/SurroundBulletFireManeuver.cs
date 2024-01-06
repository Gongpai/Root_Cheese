namespace GDD
{
    public class SurroundBulletFireManeuver : BulletFireManeuver
    {
        public override void Start()
        {
            base.Start();
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
                BulletShotSurroundMode.Surround,
                BulletShotMode.SurroundMode
            );
        }
    }
}