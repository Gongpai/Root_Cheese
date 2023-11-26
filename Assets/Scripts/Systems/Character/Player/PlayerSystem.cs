using UnityEngine;

namespace GDD
{
    public class PlayerSystem : HealthSystem
    {
        [SerializeField][Tooltip("Time For Delay Enter Attack State")] 
        private float m_delay_attack = 0.5f;
        
        private PlayerCharacterController _playerController;

        private IState<PlayerSystem> _attackState, _moveState;

        private StateContext<PlayerSystem> _playerStateContext;

        public float delay_attack
        {
            get => m_delay_attack;
        }
        // Start is called before the first frame update
        void Start()
        {
            _playerController = GetComponent<PlayerCharacterController>();

            _playerStateContext = new StateContext<PlayerSystem>(this);

            _attackState = gameObject.AddComponent<PlayerAttackState>();
            _moveState = gameObject.AddComponent<PlayerMoveState>();
        }

        // Update is called once per frame
        void Update()
        {
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
    }
}