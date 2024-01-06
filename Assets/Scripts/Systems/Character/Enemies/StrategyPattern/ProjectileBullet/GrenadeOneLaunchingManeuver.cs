using GDD.Spatial_Partition;

namespace GDD
{
    public class GrenadeOneLaunchingManeuver : GrenadeLaunchManeuver
    {
        public override void Start()
        {
            base.Start();
        }
        
        public override void OnFire(IPawn enemy)
        {
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
        
        public override void ToggleFire(EnemySpawnBullet enemySpawnBullet, int[] posIndex = default)
        {
            base.ToggleFire(enemySpawnBullet, posIndex);
        }
    }
}