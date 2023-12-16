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
        private GameManager GM;
        private Vector3 oldPos;
        
        private IState<EnemySystem> _attackState, _moveState;
        private IState<EnemySystem> _currentState;
        private StateContext<EnemySystem> _enemyStateContext;
        private WaypointReachingState _waypointReaching;
        private GameObject _waypoint;

        private void Awake()
        {
            _enemyStateContext = new StateContext<EnemySystem>(this);
            _moveState = gameObject.AddComponent<EnemyMoveState>();
            _attackState = gameObject.AddComponent<EnemyAttackState>();
        }

        public override void OnEnable()
        {
            base.OnEnable();
            
            _waypointReaching = GetComponent<WaypointReachingState>();
        }

        public override void Start()
        {
            base.Start();
            GM = GameManager.Instance;
            GM.enemies.Add(this);
            
            //Add this unit to the grid
            GM.grid.Add(this);

            oldPos = transform.position;
        }

        public override void Update()
        {
            base.Update();

            if (_currentState == null)
                _currentState = _attackState;
            
            _enemyStateContext.Transition(_currentState);
            
            UpdateEnemyMove();
        }

        public void StartAttack()
        {
            if (_attackState != null)
            {
                _currentState = _attackState;
            }
        }

        public void StartMove()
        {
            RandomWayPointPosition();
            _currentState = _moveState;
        }

        protected void RandomWayPointPosition()
        {
            if (_waypoint == null && this.enabled)
            {
                _waypoint = new GameObject(gameObject.name + " WayPoint");
                _waypoint.transform.parent = transform.parent;
                _waypointReaching._waypoints.Add(_waypoint.transform);
            }

            Vector3 randomDirection = Random.insideUnitSphere * (GM.mapWidth / 2);
            randomDirection += new Vector3(GM.mapWidth / 2, transform.position.y, GM.mapWidth / 2);
            NavMeshHit _hit;
            NavMesh.SamplePosition(randomDirection, out _hit, (GM.mapWidth / 2), 1);

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