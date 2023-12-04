using System;
using System.Collections.Generic;
using GDD.Util;
using UnityEngine;

namespace GDD
{
    public class ProjectileReflectionBulletCalculate : MonoBehaviour
    {
        private int maxReflexCount = 5;
        private float maxStepDistance = 50.0f;
        private float time = 0;
        private GameObject bullet;
        private ProjectileReflectionLines _projectileReflectionLines;
        private bool is_line_null = true;
        [SerializeField]private bool is_start;

        private Vector3 _reflex_DirStart;
        private Vector3 _reflex_PosStart;
        private List<Vector3> _reflexPos = new List<Vector3>();
        private List<Vector3> _reflexDir = new List<Vector3>();
        
        private int L_Default;
        private int L_Bullet;
        private int L_Character;
        private int L_Enemy;

        private void Awake()
        {
            _projectileReflectionLines = gameObject.AddComponent<ProjectileReflectionLines>();
        }

        private void Start()
        {
            L_Default = LayerMask.NameToLayer("Default");
            L_Bullet = LayerMask.NameToLayer("Bullet");
            L_Character = LayerMask.NameToLayer("Character");
            L_Enemy = LayerMask.NameToLayer("Enemy");
        }

        private void Update()
        {
            if (is_start)
                ProjectileReflection();

            if (!_projectileReflectionLines.is_null && !is_start)
            {
                _projectileReflectionLines.ClearLine();
                is_line_null = _projectileReflectionLines.is_null;
            }
        }

        private void ProjectileReflection()
        {
            //Set Start Position And Direction
            Vector3 pos = transform.position + transform.forward * 0.75f;
            Vector3 dir = transform.forward;
            _reflex_DirStart = dir;
            
            //For Debug Only vvvvvvvvvv
            //CreateBulletObjectDebug();
            
            //Clear List Position And Direction
            _reflexPos = new List<Vector3>();
            _reflexDir = new List<Vector3>();
            
            //Build Projectile Reflection Line 
            for (int i = 0; i < maxReflexCount; i++)
            {
                Vector3 startPos = pos;
                Ray ray = new Ray(pos, dir);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, maxStepDistance, 1<<L_Default|0<<L_Bullet|0<<L_Character|0<<L_Enemy))
                {
                    dir = Vector3.Reflect(dir, hit.normal);
                    //print("OffSet" + reflex_offset);
                    pos = VectorUtil.GetVectorDistance(pos, hit.point, 0.25f);
                    //pos = hit.point - new Vector3(Vector3.Magnitude(reflex_offset), 0, Vector3.Magnitude(reflex_offset)) * 0.5f;
                }
                else
                {
                    pos += dir * maxStepDistance;
                }

                if (is_line_null)
                {
                    _projectileReflectionLines.AddLine(startPos, pos);
                }
                else
                {
                    _projectileReflectionLines.UpdateLinePosition(startPos, pos, i);
                }

                _reflexPos.Add(pos);
                _reflexDir.Add(dir);
            }
            
            //For Debug Only vvvvvvvvvv
            /*
            //Add Projectile Reflection Data TO ProjectileReflectionBullet Component
            if (bullet != null && bullet.GetComponent<ProjectileReflectionBullet>() != null)
            {
                bullet.GetComponent<ProjectileReflectionBullet>().reflexPos = _reflexPos;
                bullet.GetComponent<ProjectileReflectionBullet>().reflexDir = _reflexDir;
                bullet = null;
            }
            */
            
            //Check When Projectile Reflection Lines Is Empty
            is_line_null = _projectileReflectionLines.is_null;
        }

        private void CreateBulletObjectDebug()
        {
            //Spawn Bullet Test
            if (time <= 0)
            {
                print("Create Obj Test Projectile ReflectionBulletCalculate");
                time = 30;
                bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                bullet.transform.name = "Test Projectile ReflectionBulletCalculate";
                bullet.layer = L_Bullet;
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;
                bullet.transform.localScale = Vector3.one * 0.25f;
                bullet.GetComponent<Collider>().isTrigger = true;
                bullet.AddComponent<Rigidbody>();
                bullet.AddComponent<ProjectileReflectionBullet>();
            }
            else
            {
                time -= Time.deltaTime;
            }
        }
        
        public void OnStart()
        {
            is_start = true;
        }

        public void OnStop()
        {
            is_start = false;
        }

        public Vector3 Get_FirstDirection()
        {
            return _reflex_DirStart;
        }

        public Vector3 Get_FirstPosition()
        {
            return _reflex_PosStart;
        }
        
        public List<Vector3> GetReflectionPoint()
        {
            return _reflexPos;
        }

        public List<Vector3> GetReflectionDirection()
        {
            return _reflexDir;
        }
        
        /*
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.25f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(startPos, pos);
        }*/
    }
}