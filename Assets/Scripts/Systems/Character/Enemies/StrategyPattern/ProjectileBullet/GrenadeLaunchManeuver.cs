using System.Collections;
using GDD.PUN;
using GDD.Spatial_Partition;
using UnityEngine;

namespace GDD
{
    public class GrenadeLaunchManeuver : BulletFireManeuver
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void ToggleFireToEvent()
        {
            if (!_haspunECC)
            {
                print("Not Has PunECC");
                OnFireEvent = () => { ToggleFire(_enemySpawnBullet); };
            }
            else
            {
                OnFireEvent = () =>
                {
                    int[] posIndex = RandomPositionTargetFromCustomProperties(m_enemyBulletConfig.shot);

                    ToggleFire(_enemySpawnBullet, posIndex);
                    _punECC.CallRaiseToggleFireEvent(m_enemyBulletConfig.bulletType, posIndex);
                };
            }
        }

        public override void Start()
        {
            base.Start();
        }
        
        public override void OnFire(IPawn enemy)
        {
            base.OnFire(enemy);
        }
        
        public override void ToggleFire(EnemySpawnBullet enemySpawnBullet, int[] posIndex = default)
        {
            base.ToggleFire(enemySpawnBullet);
            
            //print($"Fire us null {enemySpawnBullet.bulletObjectPool == null}!");
            
            enemySpawnBullet.bulletObjectPool.Set_GameObject = bullet;
            
            EnemyBulletConfig _enemyBulletConfig = m_enemyBulletConfig;
            enemySpawnBullet.OnSpawnGrenade(
                _enemyBulletConfig.shot,
                _enemyBulletConfig.damage,
                posIndex
                );
        }

        protected int[] RandomPositionTargetFromCustomProperties(int shot)
        {
            int[] posIndexs = new int[shot];

            for (int i = 0; i < posIndexs.Length; i++)
            {
                posIndexs[i] = Random.Range(0, PunGameSetting.RandomPositionTargetCount - 1);
            }

            return posIndexs;
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