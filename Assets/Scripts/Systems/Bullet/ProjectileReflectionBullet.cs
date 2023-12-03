using System;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class ProjectileReflectionBullet : MonoBehaviour
    {
        private Rigidbody rig;
        //Ray ray;
        //RaycastHit hit;
        private bool is_reflex;
        Vector3 velocity;
        private float dis;
        private float old_dis = 0;
        private float _power = 3;

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

        public float power
        {
            set => _power = value;
        }

        private int ReflexCount = 0;
        
        private void Start()
        {
            rig = GetComponent<Rigidbody>();
            rig.useGravity = false;
            rig.AddForce(transform.forward * _power, ForceMode.Impulse);
        }

        private void Update()
        {
            //print(ReflexCount + " Obj Distance : " + Vector3.Distance(_reflexPos[ReflexCount], transform.position) + " | Snap Distance : " + rig.velocity.magnitude * Time.deltaTime);
            if (ReflexCount < 5 && Vector3.Distance(_reflexPos[ReflexCount], transform.position) <= rig.velocity.magnitude * Time.deltaTime)
            {
                //print("On Reflex!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                if (!is_reflex)
                {
                    is_reflex = true;
                    rig.velocity = reflexDir[ReflexCount] * _power;
                    transform.position = _reflexPos[ReflexCount];

                    //rig.AddForce(reflexDir[ReflexCount] * 2.0f, ForceMode.Impulse);

                    ReflexCount++;
                    //print("REflexxxxxxxxxxxxxxxxxxx : " + Vector3.Distance(transform.position, hit.point));
                }
            }
            else
            {
                is_reflex = false;
            }
            
            if (ReflexCount > 4)
                Destroy(gameObject);
        }

        private void OnDestroy()
        {
            print("Deadddddddddddddddddddddddddddddddddddddddd");
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