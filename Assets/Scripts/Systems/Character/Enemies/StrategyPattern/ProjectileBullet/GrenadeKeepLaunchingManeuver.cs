using GDD.Spatial_Partition;

namespace GDD
{
    public class GrenadeKeepLaunchingManeuver : GrenadeLaunchManeuver
    {
        public override void Start()
        {
            base.Start();
        }
        
        public override void ToggleFire(EnemySpawnBullet enemySpawnBullet, int[] posIndex = default)
        {
            base.ToggleFire(enemySpawnBullet, posIndex);
        }
    }
}