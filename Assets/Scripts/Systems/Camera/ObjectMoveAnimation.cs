using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public class ObjectMoveAnimation : MonoBehaviour
    {
        public int originalIndex;
        public float time;
        private List<Vector3> m_positions;
        private Animator _animator;
        private Vector3 movePos;
        private Vector3 currentPos;
        private bool isCanMove = true;
        private UnityAction _onMoveEnd;
        private AnimationStateAction ASA;

        public UnityAction OnMoveEnd
        {
            get => _onMoveEnd;
            set => _onMoveEnd = value;
        }

        public List<Vector3> positions
        {
            set => m_positions = value;
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            ASA = _animator.GetBehaviour<AnimationStateAction>();
        }

        private void OnEnable()
        {
            ASA.OnStateUpdateAction += MoveEndAction;
        }

        private void Start()
        {
            currentPos = transform.localPosition;
        }

        private void Update()
        {
            if (time >= 1)
            {
                currentPos = transform.localPosition;
                transform.localPosition = currentPos;
                
                isCanMove = true;
            }
            else
            {
                transform.localPosition = Vector3.Lerp(currentPos, movePos, time);
            }
        }

        private void MoveEndAction(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).length <=
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                print($"Invoke State");
                _onMoveEnd?.Invoke();
            }
        }

        public void Move(int index)
        {
            if(!isCanMove)
                return;
            
            isCanMove = false;
            
            movePos = m_positions[index];
            originalIndex = index;
            _animator.SetTrigger("Play");
            time = 0;
        }

        private void OnDisable()
        {
            ASA.OnStateUpdateAction -= MoveEndAction;
        }
    }
}