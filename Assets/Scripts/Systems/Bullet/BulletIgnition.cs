using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine.Utility;
using GDD.ObjectPool;
using GDD.PUN;
using GDD.Util;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace GDD
{
    public class BulletIgnition : MonoBehaviour
    {
        [SerializeField] private GameObject _attackVFX;
        [SerializeField] protected Transform m_spawnPoint;
        private GameObject bullet_rot_spawn;
        protected List<GameObject> bullets;
        private SpawnerProjectileReflectionBulletCalculate _SPRBC;
        private ProjectileLauncherCalculate _PLC;
        private List<Quaternion> rots_random = new List<Quaternion>();
        private GameManager GM;
        private VFXSpawner _vfxSpawner;
        private GameObject group_launcher_point;

        public Transform spawnPoint
        {
            get => m_spawnPoint;
        }

        private void Awake()
        {
            GM = GameManager.Instance;
            _vfxSpawner = gameObject.AddComponent<VFXSpawner>();
            _vfxSpawner.Set_GameObject = _attackVFX;
        }

        public virtual void Start()
        {
            /*string sb = " ␆ ␈ ␇ ␘ ␍ ␐ ␡ ␔ ␑ ␓ ␒ ␙ ␃ ␄ ␗ ␅ ␛ ␜ ␌ ␝ ␉ ␊ ␕ ␤ ␀ ␞ ␏ ␎ ␠ ␁ ␂ ␚ ␖ ␟ ␋";
            //print(sb);*/
        }

        public virtual void Update()
        {
            
        }

        public virtual List<GameObject> OnSpawnBullet(float distance, float power, int shot, float damge, Transform target, BulletType type, BulletShotSurroundMode surroundMode, BulletShotMode shotMode, ObjectPoolBuilder builder = null)
        {
            if (shotMode == BulletShotMode.SurroundMode)
                return OnIgnitionBulletSurround(builder,
                    m_spawnPoint,
                    target,
                    type,
                    distance,
                    power,
                    shot,
                    damge,
                    surroundMode);
            else
                return OnIgnitionBulletRandom(builder, m_spawnPoint, target, distance, power, shot, damge);
        }

        public virtual void OnSpawnGrenade(int shot, float damge, int[] posIndex = default, ObjectPoolBuilder builder = null)
        {
            //print($"builder : {builder == null} | m_spawnPoint : {m_spawnPoint == null} | shot : {shot} | damge : {damge} | posIndex : {posIndex == null}");
            
            OnProjectileLaunch(builder, m_spawnPoint, shot, damge, posIndex);
        }

        public List<GameObject> OnIgnitionBulletSurround(ObjectPoolBuilder builder, Transform spawnPoint, Transform target, BulletType _type, float distance, float power, int shot, float damage, BulletShotSurroundMode surroundMode)
        {
            float current_axis = 0;
            float surrounded_axis;
            int helf_axis = 0;
            if (surroundMode == BulletShotSurroundMode.Surround)
            {
                surrounded_axis = 360.0f / shot;
            }
            else
            {
                helf_axis = 180 / shot;
                surrounded_axis = helf_axis;
            }

            bullets = new List<GameObject>();
            
            //Aim to target
            Quaternion lookAt = Quaternion.LookRotation(target.position - spawnPoint.position);
            current_axis += lookAt.eulerAngles.y - transform.eulerAngles.y;
            
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
                bullet_rot_spawn.transform.localPosition = bullet_rot_spawn.transform.forward * distance;
                bullet_rot_spawn.transform.rotation = Quaternion.identity;
                
                if(_type == BulletType.Rectilinear)
                    AddForceBullet(builder, spawnPoint, power, damage);
                else if (_type == BulletType.ProjectileReflection)
                    CreateProjectileReflectionBullet(builder, spawnPoint, power, damage, i);

                GameObject vfxObject = _vfxSpawner.OnSpawn();
                vfxObject.transform.position = bullet_rot_spawn.transform.position;
                vfxObject.transform.rotation = spawnPoint.rotation;

                //Add Axis
                current_axis += surrounded_axis;
            }

            return bullets;
        }
        
        public List<GameObject> OnIgnitionBulletRandom(ObjectPoolBuilder builder, Transform spawnPoint, Transform target, float distance, float power, int shot, float damage)
        {
            int current_axis = 0;
            int surrounded_axis;
            int helf_axis = 180 / shot;
            surrounded_axis = helf_axis;

            bullets = new List<GameObject>();
            
            if(bullet_rot_spawn == null)
                bullet_rot_spawn = new GameObject("bullet rot spawn");
            
            spawnPoint.rotation = Quaternion.identity;
            bullet_rot_spawn.transform.parent = spawnPoint;
            bullet_rot_spawn.transform.localPosition = Vector3.zero;
            bullet_rot_spawn.transform.rotation = Quaternion.identity;
            
            if (rots_random.Count <= 0)
            {
                for (int i = 0; i < shot; i++)
                {
                    //Rotation Parent Spawn Point
                    Quaternion rot = Quaternion.AngleAxis(current_axis, Vector3.up);
                    spawnPoint.rotation = transform.rotation * Quaternion.Euler(new Vector3(0, (helf_axis / 2) + 90, 0));
                    spawnPoint.rotation *= rot;
                    rots_random.Add(spawnPoint.rotation);

                    //Rot
                    bullet_rot_spawn.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    bullet_rot_spawn.transform.rotation = Quaternion.identity;
                    bullet_rot_spawn.transform.localPosition = bullet_rot_spawn.transform.forward * distance;
                    
                    //Add Axis
                    current_axis += surrounded_axis;
                }
            }
            
            float random_rot = Random.Range(-30, 30);
            //print("Random Rot : " + random_rot);
                    
            foreach (var rot in rots_random)
            {
                //print("ROTOTOTOTOTOTOTO : " + rot.eulerAngles);
                //Add Force And Spawn Bullet
                GameObject bullet = builder.OnSpawn();
                bullet.GetComponent<TakeDamage>().damage = damage;
                bullet.GetComponent<Collider>().isTrigger = true;
                
                //Set Rot
                bullet.transform.rotation = Quaternion.identity;
                bullet.transform.position = bullet_rot_spawn.transform.position;

                Quaternion rot_final = transform.rotation * rot;
                bullet.transform.rotation = Quaternion.Euler(new Vector3(0, rot_final.eulerAngles.y, 0));
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
                
                //Spawn VFX
                GameObject vfxObject = _vfxSpawner.OnSpawn();
                vfxObject.transform.position = bullet_rot_spawn.transform.position;
                vfxObject.transform.rotation = spawnPoint.rotation;
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
            _prBullet.reflexDir = _SPRBC.PRBCs[index].GetReflectionDirection();
            _prBullet.reflexPos = _SPRBC.PRBCs[index].GetReflectionPoint();
            
            print($"bullet is null : {bullet == null}");
            print($"_prBullet is null : {_prBullet == null}");
            print($"_prBullet.reflexDir is null : {_prBullet.reflexDir == null}");
            bullet.transform.position += _prBullet.reflexDir[0] * 1.25f;
            
            bullet.GetComponent<Collider>().isTrigger = true;
            bullet.transform.position = bullet_rot_spawn.transform.position;
            _prBullet.OnIgnition();

            //Add Bullet To List
            bullets.Add(bullet);
        }

        public void OnProjectileLaunch(ObjectPoolBuilder builder, Transform spawnPoint, int shot, float damage, int[] posIndex = default)
        {
            EnemySystem _enemySystem = builder.GetComponent<EnemySystem>();

            if (_PLC == null)
            {
                group_launcher_point = new GameObject(gameObject.name + " | Group Launcher Point");
                group_launcher_point.transform.parent = transform.parent;
                group_launcher_point.transform.localPosition = Vector3.zero;

                Transform launcher = null;
                if(spawnPoint.childCount > 0) 
                    launcher = spawnPoint.GetChild(0);
                if (spawnPoint.childCount <= 0 || launcher == null && launcher.name != "launcher")
                {
                    _PLC = new GameObject("launcher").AddComponent<ProjectileLauncherCalculate>();
                    _PLC.transform.parent = spawnPoint;
                }
                else
                {
                    _PLC = launcher.GetComponent<ProjectileLauncherCalculate>();
                }

                _PLC.transform.localPosition = Vector3.zero;
                _PLC.launchAngle = 70f;
            }

            for (int i = 0; i < shot; i++)
            {
                ShootingTargetObjectPool _shootingTarget = _PLC.GetComponent<ShootingTargetObjectPool>();
                if (_shootingTarget == null)
                    _shootingTarget = _PLC.gameObject.AddComponent<ShootingTargetObjectPool>();
                
                GameObject shooting_point = _shootingTarget.OnSpawn();
                shooting_point.transform.parent = group_launcher_point.transform;
                shooting_point.transform.localPosition = Vector3.zero + new Vector3(0, 0.1f, 0);
                shooting_point.GetComponent<Target_Point>().OnStart(2f);

                GameObject grenade = builder.OnSpawn();
                grenade.GetComponent<TakeDamage>().damage = damage;
                grenade.GetComponent<Collider>().isTrigger = true;

                grenade.transform.position = _PLC.transform.position;
                Rigidbody rig = grenade.GetComponent<Rigidbody>();
                rig.velocity = Vector3.zero;

                Vector3 posToTarget;
                if (GM.playMode == PlayMode.Singleplayer)
                    posToTarget = GM.players.Keys.ElementAt(_enemySystem.targetID).transform.position;
                else
                {
                    PhotonView PtvTarget = PhotonNetwork.GetPhotonView(_enemySystem.targetID);

                    if (PtvTarget == null)
                        return;
                    else
                        posToTarget = PtvTarget.transform.position;
                }
                
                if (i > 0)
                {
                    if (GM.playMode == PlayMode.Singleplayer)
                        posToTarget = RandomPositionTargetProjectileLaunch(posToTarget);
                    else
                        posToTarget =
                            RandomPositionTargetProjectileLaunchCustomProperties(posToTarget, posIndex[i]);
                }

                shooting_point.transform.position = new Vector3(posToTarget.x, shooting_point.transform.position.y, posToTarget.z);
                
                Vector3 _velocity = _PLC.GetVelocityProjectile(transform.position, posToTarget, 0.1f);
                
                if(_velocity.IsNaN())
                    return;

                rig.velocity = _velocity;
            }
        }

        private Vector3 RandomPositionTargetProjectileLaunch(Vector3 playerTarget)
        {
            Vector2 vector_random = Random.insideUnitCircle * 5f;
            Vector3 player_random = new Vector3(vector_random.x, 0, vector_random.y);
            return player_random + playerTarget;
        }

        private Vector3 RandomPositionTargetProjectileLaunchCustomProperties(Vector3 playerTarget, int posIndex)
        {
            float2D vector_random = PunGameSetting.Pre_RandomTargetPosition[posIndex];
            
            Vector3 player_random = new Vector3(vector_random.x, 0, vector_random.y);
            return player_random + playerTarget;
        }
    }
}