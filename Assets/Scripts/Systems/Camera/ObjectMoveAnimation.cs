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
        private UnityAction _onMoveEnd;
        private int currentMove;
        private AnimationStateAction ASA;
        private bool _canMove;

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
            ASA.OnStateExitAction += MoveEndAction;
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
            }
            else
            {
                transform.localPosition = Vector3.Lerp(currentPos, movePos, time);
            }
        }

        private void MoveEndAction(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _onMoveEnd?.Invoke();
        }

        public void Move(int index, int move)
        {
            print("Moveee Nowwwws");
            currentMove = move;
            movePos = m_positions[index];
            _animator.SetTrigger("Play");
            time = 0;
            _canMove = false;
        }

        private void OnDisable()
        {
            ASA.OnStateExitAction -= MoveEndAction;
        }
    }
}