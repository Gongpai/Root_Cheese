using System;
using System.Linq;
using GDD.PUN;
using GDD.Spatial_Partition;
using GDD.StateMachine;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace GDD
{
    public class EnemySystem : CharacterSystem
    {
        [Header("Drop Item")] 
        [SerializeField] private int _dropEXP = 30;
        
        private GameManager GM;
        private Vector3 oldPos;
        
        private IState<EnemySystem> _attackState, _moveState;
        private IState<EnemySystem> _currentState;
        private StateContext<EnemySystem> _enemyStateContext;
        private WaypointReachingState _waypointReaching;
        private PunEnemyCharacterController _punECC;
        private DropItemObjectPool _dropItemObject;
        private GameObject _waypoint;
        private int _targetID = 0;

        public int targetID
        {
            get => _targetID;
            set => _targetID = value;
        }
        
        private void Awake()
        {
            //Add AI Enemy to GameManager
            GM = GameManager.Instance;
            GM.enemies.Add(this);
            _punECC = GetComponent<PunEnemyCharacterController>();
            
            if(!_isMasterClient)
                return;
            
            _enemyStateContext = new StateContext<EnemySystem>(this);
            _moveState = gameObject.AddComponent<EnemyMoveState>();
            _attackState = gameObject.AddComponent<EnemyAttackState>();
            _dropItemObject = GetComponent<DropItemObjectPool>();
        }

        public override void OnEnable()
        {
            base.OnEnable();
            
            _waypointReaching = GetComponent<WaypointReachingState>();
        }

        public override void Start()
        {
            base.Start();
            
            //Add this unit to the grid
            GM.grid.Add(this);

            oldPos = transform.position;
        }

        public override void Update()
        {
            base.Update();

            if(!_isMasterClient)
                return;
            
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

        public int SetTargetRandom()
        {
            if (GM.playMode == PlayMode.Singleplayer)
            {
                _targetID = Random.Range(0, GM.players.Count - 1);
                return _targetID;
            }
            else
            {
                int i;
                if (GM.players.Count > 1)
                    i = Mathf.FloorToInt(Random.Range(0.0f, GM.players.Count * 200.0f) / 200.0f);
                else
                    i = 0;
                print($"I Random : ({i}) || Player Count : {GM.players.Count}");
                MonoBehaviourPun _monoBehaviourPun = GM.players.Keys.ElementAt(i).GetComponent<MonoBehaviourPun>();
                
                _targetID = _monoBehaviourPun.photonView.ViewID;
                _punECC.OnUpdateTargetID(_targetID);
                return targetID;
            }
        }

        protected void RandomWayPointPosition()
        {
            if(!_isMasterClient)
                return;
            
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

        public override void OnCharacterDead()
        {
            base.OnCharacterDead();
            
            _dropItemObject.OnCreateObject();
            AddEXPToPlayer();
        }

        protected void AddEXPToPlayer()
        {
            if(GM.playMode == PlayMode.Singleplayer || GM.players.Count <= 1)
                GM.players.Keys.ElementAt(0).AddEXP(_dropEXP);
            else
            {
                GM.players.Keys.ElementAt(0).AddEXP(_dropEXP / 2);
                GM.players.Keys.ElementAt(1).AddEXP(_dropEXP / 2);
            }
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

            GM.enemies.Remove(this);
            
            if(_waypoint != null)
                Destroy(_waypoint);
        }
    }
}