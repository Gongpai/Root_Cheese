using System;
using UnityEngine;

namespace Systems.Shader
{
    public class MaterialAnimationController : MonoBehaviour
    {
        [Header("Loop")]
        [SerializeField] private bool m_loop;
        [SerializeField] private string m_loopName = "_Move";
        [SerializeField] private ValueType m_type;
        [SerializeField] private float m_loopLength = 1;
        [SerializeField] private float m_loopTime = 1;
        [SerializeField] private float m_speed = 1;
        
        [Header("Color")]
        [SerializeField] private bool m_setColor = true;
        [SerializeField] private string m_colorName = "_Color";
        [SerializeField] private Color m_color;
        private Material _material;
        private float _valueLoop = 0;

        [Serializable]
        enum ValueType
        {
            Float,
            Int
        }

    private void Start()
        {
            _material = GetComponent<Renderer>().sharedMaterial;

            if (!m_setColor)
            {
                return;
            }
            _material.SetColor(m_colorName, m_color);
        }

        private void Update()
        {
            LoopTime();
        }
        
        public void LoopTime()
        {
            if(!m_loop)
                return;
            
            if (Mathf.Abs(_valueLoop) < m_loopLength)
                _valueLoop += Time.deltaTime * m_speed * m_loopTime;
            else if (m_loop)
                _valueLoop = 0;
            else
                _valueLoop = m_loopLength * m_loopTime;

            switch (m_type)
            {
                case ValueType.Float:
                    _material.SetFloat(m_loopName, _valueLoop);
                    break;
                case ValueType.Int:
                    _material.SetInt(m_loopName, Mathf.FloorToInt(_valueLoop));
                    break;
            }
        }
    }
}