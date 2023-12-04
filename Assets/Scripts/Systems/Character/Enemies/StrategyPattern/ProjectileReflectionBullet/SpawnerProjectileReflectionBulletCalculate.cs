using System;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class SpawnerProjectileReflectionBulletCalculate : MonoBehaviour
    {
        private List<ProjectileReflectionBulletCalculate> _PRBCs = new List<ProjectileReflectionBulletCalculate>();
        private int _shot = 0;
        private BulletShotSurroundMode _surroundMode;
        private int current_axis = 0;
        private int surrounded_axis;
        private int helf_axis = 0;
        private GameObject spawnPoint;
        private Quaternion rot_dir;

        public BulletShotSurroundMode surroundMode
        {
            set => _surroundMode = value;
        }
        
        public int shot
        {
            set => _shot = value;
        }

        public GameObject Set_spawnPoint
        {
            set => spawnPoint = value;
        }

        public List<ProjectileReflectionBulletCalculate> PRBCs
        {
            get => _PRBCs;
        }

        public void OnStart()
        {
            print("OnStarttttttttttttttttttt");
            
            if (_surroundMode == BulletShotSurroundMode.Surround)
            {
                surrounded_axis = 360 / _shot;
            }
            else
            {
                helf_axis = 180 / _shot;
                surrounded_axis = helf_axis;
            }

            for (int i = 0; i < _shot; i++)
            {
                //Rotation Parent Spawn Point
                Quaternion rot = Quaternion.AngleAxis(current_axis, Vector3.up);

                if (_surroundMode == BulletShotSurroundMode.Surround)
                    rot_dir = transform.rotation;
                else if (_surroundMode == BulletShotSurroundMode.Front)
                    rot_dir = transform.rotation * Quaternion.Euler(new Vector3(0, (helf_axis / 2) - 90, 0));
                else if (_surroundMode == BulletShotSurroundMode.Back)
                    rot_dir = transform.rotation * Quaternion.Euler(new Vector3(0, (helf_axis / 2) + 90, 0));
                
                rot_dir *= rot;
                CreateParentProjectileReflectionBullet(i);
                
                current_axis += surrounded_axis;
            }
        }

        private void CreateParentProjectileReflectionBullet(int index)
        {
            GameObject PPR = new GameObject((index + 1) + " Spawn Projectile Reflection Point");
            PPR.transform.parent = transform;
            PPR.transform.localPosition = Vector3.zero;
            PPR.transform.position = spawnPoint.transform.position;
            PPR.transform.localRotation = rot_dir;
            ProjectileReflectionBulletCalculate _PRBC = PPR.AddComponent<ProjectileReflectionBulletCalculate>();
            _PRBC.OnStart();
            _PRBCs.Add(_PRBC);
        }
    }
}