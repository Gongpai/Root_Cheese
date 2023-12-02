using System;
using GDD.Spatial_Partition;
using GDD.StateMachine;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace GDD
{
    public class EnemySystem : CharacterSystem
    {
        [SerializeField] private EnemyBulletConfig m_enemyBulletConfig;
        private GameManager GM;
        private Vector3 oldPos;
        
        private IState<EnemySystem> _attackState, _moveState;
        private StateContext<EnemySystem> _enemyStateContext;
        private WaypointReachingState _waypointReaching;
        private GameObject _waypoint;

        public EnemyBulletConfig _enemyBulletConfig
        {
            get => m_enemyBulletConfig;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            
            _enemyStateContext = new StateContext<EnemySystem>(this);
            _attackState = gameObject.AddComponent<EnemyAttackStateMachine>();
            _moveState = gameObject.AddComponent<EnemyMoveStateMachine>();
            _waypointReaching = GetComponent<WaypointReachingState>();
        }

        public override void Start()
        {
            base.Start();
            GM = GameManager.Instance;
            
            //Add this unit to the grid
            GM.grid.Add(this);

            oldPos = transform.position;
        }

        public override void Update()
        {
            base.Update();

            UpdateEnemyMove();
        }

        public void StartAttack()
        {
            _enemyStateContext.Transition(_attackState);
        }

        public void StartMove()
        {
            RandomWayPointPosition();
            _enemyStateContext.Transition(_moveState);
        }

        protected void RandomWayPointPosition()
        {
            if (_waypoint == null)
            {
                _waypoint = new GameObject(gameObject.name + " WayPoint");
                _waypoint.transform.parent = transform.parent;
                _waypointReaching._waypoints.Add(_waypoint.transform);
            }

            Vector3 randomDirection = Random.insideUnitSphere * (GM.mapWidth - 30);
            randomDirection += transform.position;
            NavMeshHit _hit;
            NavMesh.SamplePosition(randomDirection, out _hit, (GM.mapWidth - 30), 1);

            _waypoint.transform.position = _hit.position;
        }
        
        private void UpdateEnemyMove()
        {
            //Add this unit to the grid
            GM.grid.OnPawnMove(this, oldPos);

            //Init the old pos
            oldPos = transform.position;
        }
        
        public override void OnDisable()
        {
            base.OnDisable();
            
            if (Application.isPlaying)
            {
                GM.grid.Remove(cellPos, this);
                //print("Cell Pos : " + cellPos);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            
            if(_waypoint != null)
                Destroy(_waypoint);
        }
    }
}