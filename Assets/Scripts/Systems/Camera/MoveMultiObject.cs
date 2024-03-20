using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public class MoveMultiObject : MonoBehaviour
    {
        [Header("Setting Position")]
        [SerializeField] private List<ObjectMoveAnimation> m_objects;
        [SerializeField] private List<Vector3> m_positions;
        [SerializeField] private int originPosIndex;

        [Header("Unity Events")] 
        [SerializeField] private UnityEvent<int> m_OnSelect;
        private int _select = 0;
        private int _currentSelect;
        private bool _canMove;

        public UnityEvent<int> OnSelect
        {
            get => m_OnSelect;
            set => m_OnSelect = value;
        }

        public int currentSelect
        {
            get => _currentSelect;
            set => _currentSelect = value;
        }

        public int select
        {
            get => _select;
        }

        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < m_objects.Count; i++)
            {
                m_objects[i].originalIndex = originPosIndex + i;
                m_objects[i].positions = m_positions;
            }

            /*_select = 3;
            MoveObject();*/
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                Select(1);
            }

            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                Select(-1);
            }
        }

        public void Select(int value)
        {
            _select += value;

            if (_select < 0)
            {
                _select = 0;
                return;
            }
            else if (_select > m_objects.Count - 1)
            {
                _select = m_objects.Count - 1;
                return;
            }

            m_OnSelect?.Invoke(_select);
            
            if(_canMove)
                return;
            
            MoveObject();
        }

        public void SetSelect(int chapter)
        {
            _select = chapter;
            if(_canMove)
                return;
            
            MoveObject();
        }

        public void MoveObject()
        {
            print("Move Object");
            _canMove = true;
            
            m_objects[0].OnMoveEnd += MoveCallBack;
            
            AnimationMove(_currentSelect);
        }

        private void MoveCallBack()
        {
            int moveTo = _currentSelect < _select ? 1 : -1;
            
            print($"Move to {_currentSelect}");

            if (_currentSelect != _select)
            {
                _currentSelect += moveTo;
                MoveObject();
            }
            else
                _canMove = false;
            
            m_objects[0].OnMoveEnd -= MoveCallBack;
        }

        private void AnimationMove(int move)
        {
            print($"Move to : {move}");
            
            for (int i = 0; i < m_objects.Count; i++)
            {
                int move_i = m_objects[i].originalIndex - move;
                m_objects[i].Move(move_i, move);
            }
        }

        public void OnMove(int move)
        {
            /*if((m_objects[0].originalIndex + move) < 0 || (m_objects[m_objects.Count - 1].originalIndex + move) > m_positions.Count - 1)
                return;

            //Set Select Chapter
            m_objects[0].OnMoveEnd = _move =>
            {
                if (_select + -move >= 0 && _select + -move <= m_objects.Count - 1)
                    _select += -move;
                
                m_OnSelect?.Invoke(_select);
                print($"Select i = {_select}");
            };
            
            
            if (move > 0)
            {
                for (int i = 0; i < m_objects.Count; i++)
                {
                    int move_i = m_objects[i].originalIndex + 1;
                    m_objects[i].Move(move_i, move);
                }
            }
            else
            {
                for (int i = 0; i < m_objects.Count; i++)
                {
                    int move_i = m_objects[i].originalIndex - 1;
                    m_objects[i].Move(move_i, move);
                }
            }*/
        }
    }
}