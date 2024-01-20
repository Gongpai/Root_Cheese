using System;
using System.Collections.Generic;
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

        public UnityAction OnMoveEnd
        {
            get => _onMoveEnd;
            set => _onMoveEnd = value;
        }

        public List<Vector3> positions
        {
            set => m_positions = value;
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            currentPos = transform.localPosition;
        }

        private void Update()
        {
            if (time >= 1)
            {
                currentPos = transform.localPosition;
                transform.localPosition = currentPos;
                
                if(!isCanMove)
                    _onMoveEnd?.Invoke();
                
                isCanMove = true;
            }
            else
            {
                transform.localPosition = Vector3.Lerp(currentPos, movePos, time);
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
    }
}