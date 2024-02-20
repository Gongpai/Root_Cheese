using System.Collections;
using System.Linq;
using GDD.Spatial_Partition;
using Photon.Pun;
using UnityEngine;

namespace GDD
{
    public class RandomBulletFireManeuver : BulletFireManeuver
    {
        public override void Start()
        {
            base.Start();
            
            transform.rotation = Quaternion.identity;
        }
        
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
                        _punECC.CallRaiseToggleFireEvent(m_enemyBulletConfig.bulletType, PhotonNetwork.PlayerList.ToList().IndexOf(_target.GetComponent<PhotonView>().Owner));

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
                BulletShotSurroundMode.Back,
                BulletShotMode.RandomMode
            );
        }
    }
}