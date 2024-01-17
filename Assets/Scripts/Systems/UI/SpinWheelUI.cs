using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GDD
{
    public class SpinWheelUI : MonoBehaviour
    {
        [SerializeField] private int m_count;
        private Image m_image;
        private RectTransform _rectTransform;
        private float _randomRot;

        public float rotationZ;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            _rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 1) * (rotationZ * _randomRot));
        }

        private void CreatSpinWheel()
        {
            foreach (Transform picker in transform)
            {
                Destroy(picker.gameObject);
            }
            
            _randomRot = Random.Range(580.0f, 1360.0f);
            _rectTransform.rotation = Quaternion.Euler(Vector3.zero);
            
            float degree = 360.0f;
            for (int i = 0; i < m_count; i++)
            {
                Image picker = CreatePicker($"Picker {i + 1}");
                picker.fillAmount = (degree / m_count) / degree;
                
                print($"Rotation : {(degree / m_count) * (i + 1 / m_count)}");
                picker.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, (degree / m_count) * (i + 1 / m_count)));
            }
        }

        private Image CreatePicker(string namePicker)
        {
            GameObject picker = new GameObject(namePicker);
            picker.transform.parent = transform;
            picker.transform.localPosition = Vector3.zero;

            RectTransform pickerRect = picker.AddComponent<RectTransform>();
            pickerRect.anchorMin = Vector2.zero;
            pickerRect.anchorMax = Vector2.one;
            pickerRect.anchoredPosition = Vector2.zero;
            pickerRect.sizeDelta = Vector2.zero;

            Image _imagePicker = picker.AddComponent<Image>();
            
            Color color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            _imagePicker.color = color;
            
            _imagePicker.sprite = Resources.Load<Sprite>("Images/UI/UI_Sprite_Circle_HightPixel");
            _imagePicker.type = Image.Type.Filled;
            _imagePicker.fillMethod = Image.FillMethod.Radial360;
            _imagePicker.fillOrigin = 2;

            return _imagePicker;
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(20, 20, 150, 50), "Create Pin Wheel"))
                CreatSpinWheel();
        }
    }
}