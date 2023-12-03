﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class ProjectileReflectionBulletCalculate : MonoBehaviour
    {
        private int maxReflexCount = 5;
        private float maxStepDistance = 50.0f;
        private float time = 0;
        private GameObject bullet;
        private Vector3 reflex_offset;
        
        private List<Vector3> _reflexPos = new List<Vector3>();
        private List<Vector3> _reflexDir = new List<Vector3>();
        
        private int L_Default;
        private int L_Bullet;

        private void Start()
        {
            L_Default = LayerMask.NameToLayer("Default");
            L_Bullet = LayerMask.NameToLayer("Bullet");
        }

        private Vector3 GetVectorDistance(Vector3 a, Vector3 b, float distance)
        {
            Vector3 result = a - b;
            result = Vector3.Normalize(result);
            result *= distance;
            result += b;
            return result;
        }
        
        private void OnDrawGizmos()
        {
            /*
            Handles.color = Color.red;
            Handles.ArrowHandleCap(0, transform.position + transform.forward * 0.25f, transform.rotation, 0.5f, EventType.Repaint);
            */
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.25f);

            Vector3 pos = transform.position + transform.forward * 0.75f;
            Vector3 dir = transform.forward;

            if (time <= 0)
            {
                time = 30;
                bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                bullet.transform.name = "Test Projectile ReflectionBulletCalculate";
                bullet.layer = L_Bullet;
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;
                bullet.transform.localScale = Vector3.one * 0.25f;
                reflex_offset = bullet.transform.localScale;
                bullet.GetComponent<Collider>().isTrigger = true;
                bullet.AddComponent<Rigidbody>();
                bullet.AddComponent<ProjectileReflectionBullet>();
            }
            else
            {
                time -= Time.deltaTime;
            }

            _reflexPos = new List<Vector3>();
            for (int i = 0; i < maxReflexCount; i++)
            {
                Vector3 startPos = pos;
                Ray ray = new Ray(pos, dir);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, maxStepDistance, 1<<L_Default|0<<L_Bullet))
                {
                    dir = Vector3.Reflect(dir, hit.normal);
                    //print("OffSet" + reflex_offset);
                    pos = GetVectorDistance(pos, hit.point, 0.25f);
                    //pos = hit.point - new Vector3(Vector3.Magnitude(reflex_offset), 0, Vector3.Magnitude(reflex_offset)) * 0.5f;
                }
                else
                {
                    pos += dir * maxStepDistance;
                }
                
                _reflexPos.Add(pos);
                _reflexDir.Add(dir);
                if (bullet != null && bullet.GetComponent<ProjectileReflectionBullet>() != null)
                {
                    bullet.GetComponent<ProjectileReflectionBullet>().reflexPos = _reflexPos;
                    bullet.GetComponent<ProjectileReflectionBullet>().reflexDir = _reflexDir;
                }

                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(startPos, pos); 
            }
            
        }
    }
}