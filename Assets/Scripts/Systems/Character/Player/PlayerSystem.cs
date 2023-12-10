using GDD.Spatial_Partition;
using GDD.StateMachine;
using UnityEngine;

namespace GDD
{
    public class PlayerSystem : CharacterSystem
    {
        [SerializeField][Tooltip("Time For Delay Enter Attack State")] 
        private float m_delay_attack = 0.5f;

        [SerializeField] [Tooltip("Player vision to find enemy")]
        private Vector2 vision;
        
        private PlayerCharacterController _playerController;

        private IState<PlayerSystem> _attackState, _moveState;

        private StateContext<PlayerSystem> _playerStateContext;

        private WeaponSystem _weaponSystem;

        public float delay_attack
        {
            get => m_delay_attack;
        }
        
        public float shield
        {
            get => m_shield;
            set => m_shield = value;
        }
        
        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();

            _weaponSystem = GetComponent<WeaponSystem>();
            _playerController = GetComponent<PlayerCharacterController>();
            _playerStateContext = new StateContext<PlayerSystem>(this);
            _attackState = gameObject.AddComponent<PlayerAttackStateMachine>();
            _moveState = gameObject.AddComponent<PlayerMoveStateMachine>();
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
            
            if(_playerController.Get_Player_Move)
                StartMove();
            else
                StartAttack();
        }

        public void StartAttack()
        {
            _playerStateContext.Transition(_attackState);
        }

        public void StartMove()
        {
            _playerStateContext.Transition(_moveState);
        }

        public override float GetShield()
        {
            return _weaponSystem.attachmentStats.shield;
        }

        public override void SetShiel(float shield)
        {
            _weaponSystem.attachmentStats.shield = shield;
        }

        //Spatial Partition Enemy Finding
        public override Vector2 GetPawnVision()
        {
            return vision;
        }
        public override void SetPawnVision(Vector2 vision)
        {
            this.vision = vision;
        }
    }
}