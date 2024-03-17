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
            if((m_objects[0].originalIndex + value) < 0 || (m_objects[m_objects.Count - 1].originalIndex + value) > m_positions.Count - 1)
                return;

            _select += value;
            StartCoroutine(Moveee(_select));
        }

        IEnumerator Moveee(int select)
        {
            WaitWhile isEnd = new WaitWhile(() => _canMove);
            
            while (_currentSelect == select)
            {
                _canMove = false;
                m_objects[0].OnMoveEnd = () =>
                {
                    _canMove = true;
                    if (_currentSelect < select)
                        _currentSelect++;
                    else
                        _currentSelect--;
                };
                if(_currentSelect < select)
                    AnimationMove(_currentSelect + 1);
                else
                    AnimationMove(_currentSelect - 1);

                yield return isEnd;
            }
        }

        private void AnimationMove(int index)
        {
            if (index > 0)
            {
                for (int i = 0; i < m_objects.Count; i++)
                {
                    int move_i = m_objects[i].originalIndex + 1;
                    m_objects[i].Move(move_i);
                }
            }
            else
            {
                for (int i = 0; i < m_objects.Count; i++)
                {
                    int move_i = m_objects[i].originalIndex - 1;
                    m_objects[i].Move(move_i);
                }
            }
        }
        
        public void OnMove(int move)
        {
            if((m_objects[0].originalIndex + move) < 0 || (m_objects[m_objects.Count - 1].originalIndex + move) > m_positions.Count - 1)
                return;

            //Set Select Chapter
            m_objects[0].OnMoveEnd = () =>
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
                    m_objects[i].Move(move_i);
                }
            }
            else
            {
                for (int i = 0; i < m_objects.Count; i++)
                {
                    int move_i = m_objects[i].originalIndex - 1;
                    m_objects[i].Move(move_i);
                }
            }
        }
    }
}