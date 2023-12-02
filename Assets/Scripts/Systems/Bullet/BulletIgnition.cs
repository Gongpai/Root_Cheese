using System;
using System.Collections.Generic;
using GDD.ObjectPool;
using UnityEngine;

namespace GDD
{
    public class BulletIgnition : MonoBehaviour
    {
        [SerializeField] protected Transform m_spawnPoint;
        private GameObject bullet_rot_spawn;
        protected List<GameObject> bullets;
        
        public virtual void Start()
        {
            
        }

        public virtual void Update()
        {
            
        }
        
        public virtual List<GameObject> OnSpawnBullet(float distance, float power, int shot, BulletShotSurroundMode surroundMode, ObjectPoolBuilder builder = null)
        {
            return OnIgnition(builder, m_spawnPoint, distance, power, shot, surroundMode);
        }

        public List<GameObject> OnIgnition(ObjectPoolBuilder builder, Transform spawnPoint, float distance, float power, int shot, BulletShotSurroundMode surroundMode)
        {
            int current_axis = 0;
            int surrounded_axis = 360 / shot;
            bullets = new List<GameObject>();
            
            if(bullet_rot_spawn == null)
                bullet_rot_spawn = new GameObject("bullet rot spawn");
            
            bullet_rot_spawn.transform.parent = spawnPoint;
            bullet_rot_spawn.transform.localPosition = Vector3.zero;
            bullet_rot_spawn.transform.rotation = Quaternion.Euler(Vector3.zero);
            
            for (int i = 0; i < shot; i++)
            {
                //Rotation Parent Spawn Point
                Quaternion rot = Quaternion.AngleAxis(current_axis, spawnPoint.up);
                spawnPoint.rotation = transform.rotation;
                spawnPoint.rotation *= rot;
                
                //Rot
                bullet_rot_spawn.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                bullet_rot_spawn.transform.localPosition = Vector3.forward * distance;
                bullet_rot_spawn.transform.rotation = Quaternion.Euler(Vector3.zero);
                
                //Add Force And Spawn Bullet
                GameObject bullet = builder.OnSpawn();
                bullet.GetComponent<Collider>().isTrigger = true;
                bullet.transform.position = bullet_rot_spawn.transform.position;
                Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
                if (rigidbody == null)
                {
                    bullet.AddComponent<Rigidbody>();
                }

                rigidbody.AddForce(spawnPoint.forward * power, ForceMode.Impulse);

                //Add Bullet To List
                bullets.Add(bullet);
                
                //Add Axis
                current_axis += surrounded_axis;
            }

            return bullets;
        }
    }
}