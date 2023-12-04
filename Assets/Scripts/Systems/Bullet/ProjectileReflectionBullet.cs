using System;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class ProjectileReflectionBullet : MonoBehaviour
    {
        private TakeDamage _takeDamage;
        private Rigidbody rig;
        //Ray ray;
        //RaycastHit hit;
        private bool is_reflex;
        Vector3 _velocity;
        private float dis;
        private float old_dis = 0;
        private float _power = 3;
        private int _reflexCount = 0;
        private bool is_ignition = false;

        private Vector3 _reflex_DirStart;
        private Vector3 _reflex_PosStart;
        private List<Vector3> _reflexPos;
        private List<Vector3> _reflexDir;

        public List<Vector3> reflexPos
        {
            get => _reflexPos;
            set => _reflexPos = value;
        }
        
        public List<Vector3> reflexDir
        {
            get => _reflexDir;
            set => _reflexDir = value;
        }

        public Vector3 reflex_DirStart
        {
            set => _reflex_DirStart = value;
        }
        
        public Vector3 reflex_PosStart
        {
            set => _reflex_PosStart = value;
        }

        public float power
        {
            set => _power = value;
        }

        public void OnIgnition()
        {
            _takeDamage = GetComponent<TakeDamage>();
            
            rig = GetComponent<Rigidbody>();
            rig.position = _reflex_PosStart;
            rig.useGravity = false;
            rig.AddForce(_reflex_DirStart * _power, ForceMode.Impulse);
            is_ignition = true;
        }
        
        private void Start()
        {
            
        }

        private void Update()
        {
            if(is_ignition)
                Reflection();
        }

        private void Reflection()
        {
            
            //print(ReflexCount + " Obj Distance : " + Vector3.Distance(_reflexPos[ReflexCount], transform.position) + " | Snap Distance : " + rig.velocity.magnitude * Time.deltaTime);
            if (_reflexCount < _reflexPos.Count && Vector3.Distance(_reflexPos[_reflexCount], transform.position) <= rig.velocity.magnitude * Time.deltaTime)
            {
                //print("On Reflex!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                if (!is_reflex)
                {
                    is_reflex = true;
                    rig.velocity = reflexDir[_reflexCount] * _power;
                    transform.position = _reflexPos[_reflexCount];

                    //rig.AddForce(reflexDir[ReflexCount] * 2.0f, ForceMode.Impulse);

                    _reflexCount++;
                    //print("REflexxxxxxxxxxxxxxxxxxx : " + Vector3.Distance(transform.position, hit.point));
                }
            }
            else
            {
                is_reflex = false;
            }
            
            if (_reflexCount > _reflexPos.Count - 1)
                OnDestroyBullet();
        }

        private void OnDestroyBullet()
        {
            _takeDamage.ReturnBulletToPool();
            _reflexPos = new List<Vector3>();
            _reflexDir = new List<Vector3>();
            is_ignition = false;
            _reflexCount = 0;
            is_reflex = false;

            this.enabled = false;
        }

        /*
        private void OnDrawGizmos()
        {
            //print(_reflexPos.Count);
            foreach (var r_pos in _reflexPos)
            {
                Gizmos.color = new Color(1,1,1, 0.25f);
                Gizmos.DrawSphere(r_pos, 0.25f);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, hit.point);
        }
/*
        private void OnReflex()
        {
            ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out hit, 50))
            {
                //print("Distance : " + Vector3.Distance(transform.position, hit.point));
                if (is_reflex)
                {
                velocity = Vector3.Reflect(rig.velocity, hit.normal);
                        rig.rotation = Quaternion.LookRotation(transform.position - hit.point, transform.up) *
                                       Quaternion.Euler(new Vector3(0, -90, 0));
                        rig.velocity = velocity;
                        transform.position = _reflexPos[ReflexCount];

                    velocity = Vector3.Reflect(rig.velocity, hit_reflex.normal);
                    rig.rotation = Quaternion.LookRotation(transform.position - hit_reflex.point, transform.up) *
                                   Quaternion.Euler(new Vector3(0, -90, 0));
                    rig.velocity = velocity;
                    ReflexCount--;
                    print("REflexxxxxxxxxxxxxxxxxxx : " + Vector3.Distance(transform.position, hit.point));
                }
            }
        }*/
    }
}