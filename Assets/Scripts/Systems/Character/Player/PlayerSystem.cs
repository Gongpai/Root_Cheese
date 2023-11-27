using GDD.Spatial_Partition;
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

        public float delay_attack
        {
            get => m_delay_attack;
        }
        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            
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