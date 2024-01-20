using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GDD
{
    public class MoveMultiObject : MonoBehaviour
    {
        [Header("UI")] 
        [SerializeField] private TextMeshProUGUI m_chapter;
        
        [Header("Setting Position")]
        [SerializeField] private List<ObjectMoveAnimation> m_objects;
        [SerializeField] private List<Vector3> m_positions;
        [SerializeField] private int originPosIndex;
        private int _select = 0;

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
                OnMove(1);
            }

            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                OnMove(-1);
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

                m_chapter.text = $"Chapter {_select + 1}";
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