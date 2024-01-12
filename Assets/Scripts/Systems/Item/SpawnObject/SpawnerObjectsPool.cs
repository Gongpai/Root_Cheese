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
    [RequireComponent(typeof(ProjectileLauncherCalculate))]
    public class SpawnerObjectsPool : ObjectPoolBuilder
    {
        [SerializeField] protected GameObject m_object;
        [SerializeField] protected int m_spawnCount;
        [SerializeField] protected floatMinMax m_radius;
        [SerializeField] protected bool m_isUseCustomPositions = false;
        protected ProjectileLauncherCalculate _PLC;
        protected List<GameObject> _spawnObject = new List<GameObject>();
        protected GameObject _objectPoolGroup;
        protected string objectPoolName = "ObjectPool";
        protected Vector2[] _randomPosition;

        public Vector2[] randomPosition
        {
            set => _randomPosition = value;
        }

        public override void Start()
        {
            base.Start();

            _PLC = GetComponent<ProjectileLauncherCalculate>();

            if (_PLC)
                _PLC = gameObject.AddComponent<ProjectileLauncherCalculate>();

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
            if(GUI.Button(new Rect(20, 20, 150, 35), "Create Object"))
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
                GameObject spawnObject = OnSpawn();;
                spawnObject.transform.position = transform.position;

                Rigidbody _rig = spawnObject.GetComponent<Rigidbody>();
                if (_rig == null)
                    _rig = spawnObject.AddComponent<Rigidbody>();

                _rig.velocity = Vector3.zero;
                
                if(PunGameSetting.Pre_RandomTargetPosition == null)
                    return;
                
                Vector3 posTarget = new Vector3(PunGameSetting.Pre_RandomTargetPosition[i].x, 0.1f,
                    PunGameSetting.Pre_RandomTargetPosition[i].y);
                posTarget *= 0.75f;

                _PLC.launchAngle = 85f;
                Vector3 _velocity =
                    _PLC.GetVelocityProjectile(transform.position, posTarget + transform.position);

                if (_velocity.IsNaN())
                    _velocity = Vector3.one * Random.Range(0.01f, 0.95f);
                
                _rig.velocity = _velocity;
                _rig.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }
        }
        
        public override GameObject OnSpawn()
        {
            _spawnObject = new List<GameObject>();
            GameObject spawnObject = base.OnSpawn();

            if (spawnObject.transform.parent != _objectPoolGroup.transform)
            {
                spawnObject.transform.parent = _objectPoolGroup.transform;
                spawnObject.transform.position = Vector3.zero;
            }
            
            _spawnObject.Add(spawnObject);

            return spawnObject;
        }
    }
}