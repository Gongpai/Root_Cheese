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
    public class JumpEnemySystem : EnemySystem
    {
        [Header("Jump Setting")] 
        [SerializeField] private GameObject _LandingVFX;
        [SerializeField] private Vector3 m_ColliderCenter;
        [SerializeField] private float m_ColliderRadius;
        [SerializeField] private bool m_isTigger;
        [SerializeField] private float m_damage = 10;

        private AnimationStateAction _animationStateAction;
        private bool isDeadNow;
        
        public override void Awake()
        {
            base.Awake();
        }

        public override void Start()
        {
            base.Start();
            
            _animationStateAction = m_animator.GetBehaviour<AnimationStateAction>();
            _animationStateAction.OnStateEnterAction = (arg0, info, i) =>
            {
                isDeadNow = true;
            };
        }

        public override void Update()
        {
            base.Update();

            if (GetHP() <= 0 && !isDeadNow)
                m_animator.SetBool("DeadNow", true);
            else
                m_animator.SetBool("DeadNow",false);
        }

        protected override void AddStateComponents()
        {
            _moveState = gameObject.AddComponent<EnemyJumpState>();
            EnemyJumpState _jumpstate = ((EnemyJumpState)_moveState);
            _jumpstate.landVFX = _LandingVFX;
            _jumpstate.ColliderCenter = m_ColliderCenter;
            _jumpstate.ColliderRadius = m_ColliderRadius;
            _jumpstate.isTigger = m_isTigger;
            _jumpstate.damage = m_damage;
            _attackState = gameObject.AddComponent<EnemyAttackState>();
            _dropItemObject = GetComponent<DropItemObjectPool>();
        }

        public override void StartMove()
        {
            _currentState = _moveState;
        }

        public override void OnCharacterDead()
        {
            base.OnCharacterDead();
            
            m_animator.SetBool("Jump", false);
            m_animator.SetBool("Grounded", true);
            m_animator.SetBool("FreeFall", false);
            m_animator.SetTrigger(m_deadAnimatorState);
        }
    }
}