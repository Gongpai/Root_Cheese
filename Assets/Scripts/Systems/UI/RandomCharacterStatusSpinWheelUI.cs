using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GDD
{
    public class RandomCharacterStatusSpinWheelUI : SpinWheelUI
    {
        [SerializeField] 
        private CharacterStatusPicker m_characterStatusPicker;
        private List<UnityAction<float, float>> _resultActions = new List<UnityAction<float, float>>();
        private List<Tuple<string, float, float>> m_pickers;

        public List<UnityAction<float, float>> resultActions
        {
            set
            {
                m_count = value.Count;
                _resultActions = value;
            }
        }
        
        protected override void Start()
        {
            m_pickers = m_characterStatusPicker.picker;
            
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void ShowResult()
        {
            float indexRot = m_count + (_randomRot) % -360 / (360 / m_count);
            int index = Mathf.FloorToInt(indexRot);

            Tuple<string, float, float> amount = m_pickers[index];
            _resultActions[index]?.Invoke(amount.Item2, amount.Item3);
            m_pickerTextResult.text = amount.Item1;
        }

        protected override void CreateSpinWheel()
        {
            base.CreateSpinWheel();
        }

        protected override Image CreatePicker(string namePicker, out TextMeshProUGUI pickerText)
        {
            return base.CreatePicker(namePicker, out pickerText);
        }

        protected override string GetPickerName(int i = 0)
        {
            return m_pickers[i].Item1;
        }
    }
}