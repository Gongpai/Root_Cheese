using System.Collections;
using GDD.Spatial_Partition;
using UnityEngine;

namespace GDD
{
    public class RandomBulletFireManeuver : BulletFireManeuver
    {
        public override void OnFire(IPawn enemy)
        {
            if (enemy != null)
            {
                print("On Maneuver");
                _coroutines.Add(StartCoroutine(Waiting(
                    () => { _coroutines.Add(
                        StartCoroutine(StrafeFiring(7,1, 0.1f/*m_enemyBulletConfig.rate*/)));
                    },
                    m_enemyBulletConfig.timedelay)));
            }
        }

        IEnumerator StrafeFiring(int count, float time_delay, float fire_rate)
        {
            var instruction = new WaitForEndOfFrame();
            float time_count = time_delay;
            int current_count = 0;

            while (time_count > 0)
            {
                if (time_count == time_delay)
                {
                    while (current_count <= count)
                    {
                        yield return new WaitForSeconds(fire_rate);
                        ToggleFire(_enemySpawnBullet);

                        current_count++;
                    }
                }

                if(current_count == count)
                    time_count -= Time.deltaTime;
                //print("Time IEnumerator : " + time_count);
                
                if (time_count <= 0)
                {
                    time_count = time_delay;
                }

                yield return instruction;
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
                12,
                _enemyBulletConfig.damage,
                BulletShotSurroundMode.Surround,
                BulletShotMode.RandomMode
            );
        }
    }
}