using System;
using System.Collections.Generic;
using Cinemachine.Utility;
using GDD.ObjectPool;
using GDD.PUN;
using GDD.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GDD.Spawner
{
    public class SpawnerObjectsPool : ObjectPoolBuilder
    {
        [SerializeField] private float m_guiOffset = 20.0f;
        [SerializeField] protected GameObject m_object;
        [SerializeField] protected int m_spawnCount;
        [SerializeField] protected floatMinMax m_radius;
        [SerializeField] protected bool m_isUseCustomPositions = false;
        [SerializeField] private bool m_showGUI;
        protected ProjectileLauncherCalculate _PLC;
        protected GameObject _objectPoolGroup;
        protected string objectPoolName = "ObjectPool";
        protected Vector2[] _randomPosition;

        private ProjectileLauncherCalculate SetPLC
        {
            set => _PLC = value;
        }
        
        public Vector2[] randomPosition
        {
            set => _randomPosition = value;
        }

        protected virtual void OnEnable()
        {
             
        }

        public override void Start()
        {
            base.Start();
            
            if(_PLC == null)
                _PLC = GetComponent<ProjectileLauncherCalculate>();

            if (_objectPoolGroup == null)
            {
                _objectPoolGroup = new GameObject(objectPoolName);
                _objectPoolGroup.transform.position = Vector3.zero;
            }
            
            if(!m_isUseCustomPositions)
                RandomPosition();
        }

        public override GameObjectPool CreatePooledItem()
        {
            return base.CreatePooledItem();
        }

        private void OnGUI()
        {
            if(!m_showGUI)
                return;
            
            if(GUI.Button(new Rect(20, m_guiOffset, 150, 35), "Create Object"))
                OnCreateObject();
        }

        protected virtual void RandomPosition()
        {
            _randomPosition = new Vector2[m_spawnCount];
            
            for (int i = 0; i < m_spawnCount; i++)
            {
                _randomPosition[i] = new Vector2(Random.Range(m_radius.min, m_radius.max),
                    Random.Range(m_radius.min, m_radius.max));
            }
        }

        public virtual void OnCreateObject()
        {
            for (int i = 0; i < m_spawnCount; i++)
            {
                //GetObjectFromPool
                GameObject spawnObject = OnSpawn();
                OnCreateObjectLoop(spawnObject, i);
                
                //Set Position To Origin
                spawnObject.transform.position = transform.position;

                //Get Rigidbody
                Rigidbody _rig = spawnObject.GetComponent<Rigidbody>();
                if (_rig == null)
                    _rig = spawnObject.AddComponent<Rigidbody>();
                _rig.velocity = Vector3.zero;
                _rig.useGravity = true;
                
                //Check PunGameSetting Pre_RandomTargetPosition
                if(PunGameSetting.Pre_RandomTargetPosition == null)
                    return;
                
                //Set Pos Target
                Vector3 posTarget = GetTargetPosition(new Vector3(PunGameSetting.Pre_RandomTargetPosition[i].x, 0,PunGameSetting.Pre_RandomTargetPosition[i].y), i);
                posTarget *= 0.75f;

                //Set Angle
                _PLC.launchAngle = 85f;
                
                //Get Calculate Projectile
                Vector3 _velocity = _PLC.GetVelocityProjectile(transform.position, posTarget + transform.position);

                //Check Vector is NAN
                if (_velocity.IsNaN())
                    _velocity = Vector3.one * Random.Range(0.01f, 0.95f);
                
                //Set Start Velocity
                _rig.velocity = _velocity;
                _rig.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }
        }

        protected virtual Vector3 GetTargetPosition(Vector3 pos, int i = 0)
        {
            return pos;
        }

        protected virtual void OnCreateObjectLoop(GameObject gObject, int i)
        {
            
        }
        
        public override GameObject OnSpawn()
        {
            GameObject spawnObject = base.OnSpawn();

            if (spawnObject.transform.parent != _objectPoolGroup.transform)
            {
                spawnObject.transform.parent = _objectPoolGroup.transform;
                spawnObject.transform.position = Vector3.zero;
            }

            return spawnObject;
        }
    }
}