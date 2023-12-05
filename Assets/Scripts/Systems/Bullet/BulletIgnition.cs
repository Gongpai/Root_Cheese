﻿using System;
using System.Collections.Generic;
using GDD.ObjectPool;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace GDD
{
    public class BulletIgnition : MonoBehaviour
    {
        [SerializeField] protected Transform m_spawnPoint;
        private GameObject bullet_rot_spawn;
        protected List<GameObject> bullets;
        private SpawnerProjectileReflectionBulletCalculate _SPRBC;
        
        private List<Quaternion> rots_random = new List<Quaternion>();

        public Transform spawnPoint
        {
            get => m_spawnPoint;
        }
        
        public virtual void Start()
        {
            
        }

        public virtual void Update()
        {
            
        }
        
        public virtual List<GameObject> OnSpawnBullet(float distance, float power, int shot, float damge, BulletType type, BulletShotSurroundMode surroundMode, BulletShotMode shotMode, ObjectPoolBuilder builder = null)
        {
            if (shotMode == BulletShotMode.SurroundMode)
                return OnIgnitionBulletSurround(builder, 
                    m_spawnPoint,
                    type, 
                    distance, 
                    power, 
                    shot, 
                    damge, 
                    surroundMode);
            else
                return OnIgnitionBulletRandom(builder, m_spawnPoint, distance, power, shot, damge);
        }

        public List<GameObject> OnIgnitionBulletSurround(ObjectPoolBuilder builder, Transform spawnPoint, BulletType _type, float distance, float power, int shot, float damage, BulletShotSurroundMode surroundMode)
        {
            int current_axis = 0;
            int surrounded_axis;
            int helf_axis = 0;
            if (surroundMode == BulletShotSurroundMode.Surround)
            {
                surrounded_axis = 360 / shot;
            }
            else
            {
                helf_axis = 180 / shot;
                surrounded_axis = helf_axis;
            }

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
                
                if(surroundMode == BulletShotSurroundMode.Surround)
                    spawnPoint.rotation = transform.rotation;
                else if(surroundMode == BulletShotSurroundMode.Front)
                    spawnPoint.rotation = transform.rotation * Quaternion.Euler(new Vector3(0, (helf_axis / 2) - 90, 0));
                else if (surroundMode == BulletShotSurroundMode.Back)
                    spawnPoint.rotation = transform.rotation * Quaternion.Euler(new Vector3(0, (helf_axis / 2) + 90, 0));
                    
                
                spawnPoint.rotation *= rot;
                
                //Rot
                bullet_rot_spawn.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                bullet_rot_spawn.transform.localPosition = Vector3.forward * distance;
                bullet_rot_spawn.transform.rotation = Quaternion.Euler(Vector3.zero);
                
                if(_type == BulletType.Rectilinear)
                    AddForceBullet(builder, spawnPoint, power, damage);
                else if (_type == BulletType.ProjectileReflection)
                    CreateProjectileReflectionBullet(builder, spawnPoint, power, damage, i);

                //Add Axis
                current_axis += surrounded_axis;
            }

            return bullets;
        }
        
        public List<GameObject> OnIgnitionBulletRandom(ObjectPoolBuilder builder, Transform spawnPoint, float distance, float power, int shot, float damage)
        {
            int current_axis = 0;
            int surrounded_axis;
            int helf_axis = 180 / shot;
            surrounded_axis = helf_axis;

            bullets = new List<GameObject>();
            
            if(bullet_rot_spawn == null)
                bullet_rot_spawn = new GameObject("bullet rot spawn");
            
            bullet_rot_spawn.transform.parent = spawnPoint;
            bullet_rot_spawn.transform.localPosition = Vector3.zero;
            bullet_rot_spawn.transform.rotation = Quaternion.Euler(Vector3.zero);
            
            if (rots_random.Count <= 0)
            {
                for (int i = 0; i < shot; i++)
                {
                    //Rotation Parent Spawn Point
                    Quaternion rot = Quaternion.AngleAxis(current_axis, spawnPoint.up);
                    spawnPoint.rotation = transform.rotation * Quaternion.Euler(new Vector3(0, (helf_axis / 2) - 90, 0));
                    spawnPoint.rotation *= rot;
                    rots_random.Add(spawnPoint.rotation);

                    //Rot
                    bullet_rot_spawn.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    bullet_rot_spawn.transform.localPosition = Vector3.forward * distance;
                    bullet_rot_spawn.transform.rotation = Quaternion.Euler(Vector3.zero);
                    
                    
                    //Add Axis
                    current_axis += surrounded_axis;
                }
            }
            
            float random_rot = Random.Range(-30, 30);
            print("Random Rot : " + random_rot);
                    
            foreach (var rot in rots_random)
            {
                //print("ROTOTOTOTOTOTOTO : " + rot.eulerAngles);
                //Add Force And Spawn Bullet
                GameObject bullet = builder.OnSpawn();
                bullet.GetComponent<TakeDamage>().damage = damage;
                bullet.GetComponent<Collider>().isTrigger = true;
                
                //Set Rot
                bullet.transform.position = bullet_rot_spawn.transform.position;
                bullet.transform.rotation = transform.rotation * rot;
                bullet.transform.rotation *= Quaternion.AngleAxis(random_rot, Vector3.up);
                
                Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
                if (rigidbody == null)
                {
                    bullet.AddComponent<Rigidbody>();
                }

                rigidbody.useGravity = false;
                rigidbody.AddForce(bullet.transform.forward * power, ForceMode.Impulse);

                //Add Bullet To List
                bullets.Add(bullet);
            }

            return bullets;
        }

        private void AddForceBullet(ObjectPoolBuilder builder, Transform spawnPoint, float power, float damage)
        {
            GameObject bullet = builder.OnSpawn();
            bullet.GetComponent<TakeDamage>().damage = damage;
                
            bullet.GetComponent<Collider>().isTrigger = true;
            bullet.transform.position = bullet_rot_spawn.transform.position;
            Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                bullet.AddComponent<Rigidbody>();
            }

            rigidbody.useGravity = false;
            rigidbody.AddForce(spawnPoint.forward * power, ForceMode.Impulse);

            //Add Bullet To List
            bullets.Add(bullet);
        }

        private void CreateProjectileReflectionBullet(ObjectPoolBuilder builder, Transform spawnPoint, float power, float damage, int index)
        {
            GameObject bullet = builder.OnSpawn();
            TakeDamage _takeDamage = bullet.GetComponent<TakeDamage>();
            _takeDamage.damage = damage;
            _takeDamage.is_undying = true;
            
            ProjectileReflectionBullet _prBullet;
            if (bullet.GetComponent<ProjectileReflectionBullet>() == null)
            {
                _prBullet = bullet.AddComponent<ProjectileReflectionBullet>();
            }
            else
            {
                _prBullet = bullet.GetComponent<ProjectileReflectionBullet>();
            }
            
            _prBullet.enabled = true;
            _prBullet.power = power;

            if (_SPRBC == null)
            {
                _SPRBC = GetComponent<SpawnerProjectileReflectionBulletCalculate>();
            }

            _prBullet.reflex_DirStart = _SPRBC.PRBCs[index].Get_FirstDirection();
            _prBullet.reflex_PosStart = _SPRBC.PRBCs[index].Get_FirstPosition();
            _prBullet.reflexDir = _SPRBC.PRBCs[index].GetReflectionDirection();;
            _prBullet.reflexPos = _SPRBC.PRBCs[index].GetReflectionPoint();;
            bullet.transform.position += _prBullet.reflexDir[0] * 1.25f;
            
            bullet.GetComponent<Collider>().isTrigger = true;
            bullet.transform.position = bullet_rot_spawn.transform.position;
            _prBullet.OnIgnition();

            //Add Bullet To List
            bullets.Add(bullet);
        }
    }
}