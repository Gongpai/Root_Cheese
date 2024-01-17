using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using FontStyles = TMPro.FontStyles;
using Random = UnityEngine.Random;

namespace GDD
{
    public class SpinWheelUI : MonoBehaviour
    {
        [Header("Text")] 
        [SerializeField] private TMP_FontAsset m_fontAsset;
        [SerializeField] private FontStyles m_fontStyles;
        [SerializeField] private FontWeight m_fontWeight;
        [SerializeField] private int m_fontSize;
        [SerializeField] private Color m_fontColor;
        [SerializeField] private TextAlignmentOptions m_textAlignment;

        [Header("Picker")] 
        [SerializeField] private GameObject m_pickerResult;
        [SerializeField] private TextMeshProUGUI m_pickerTextResult;
        [SerializeField] private int m_count;
        private Image m_image;
        private RectTransform _rectTransform;
        private float _randomRot;
        private bool _isShowResult;
        private RectTransform m_textGroup;
        private List<string> m_pickerText = new List<string>();

        public float rotationZ;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            _rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 1) * (rotationZ * _randomRot));
            
            if(m_textGroup != null)
                m_textGroup.rotation = Quaternion.Euler(new Vector3(0, 0, 1) * (rotationZ * _randomRot));
            
            if(rotationZ >= 1.0f)
            {
                _isShowResult = true;
                m_pickerResult.SetActive(true);

                float indexRot = m_count + (_randomRot - 90) % -360 / (360 / m_count);
                print($"Random : {_randomRot - 90}");
                print($"Index : {indexRot}");
                m_pickerTextResult.text = m_pickerText[Mathf.FloorToInt(indexRot)];
            }
        }

        private void CreatSpinWheel()
        {
            foreach (Transform picker in transform)
            {
                Destroy(picker.gameObject);
            }

            if (m_textGroup != null)
            {
                Destroy(m_textGroup.gameObject);
                m_textGroup = null;
            }

            m_pickerText = new List<string>();
            
            _randomRot = -Random.Range(580.0f, 1360.0f);
            _rectTransform.rotation = Quaternion.Euler(Vector3.zero);
            
            float degree = 360.0f;
            for (int i = 0; i < m_count; i++)
            {
                Image picker = CreatePicker($"Picker {i + 1}", out TextMeshProUGUI _pickerText);
                picker.fillAmount = (degree / m_count) / degree;
                _pickerText.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, (degree / m_count) * -i)  - new Vector3(0, 0, 22.5f));
                
                print($"Rotation : {(degree / m_count) * (i + 1 / m_count)}");
                picker.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, (degree / m_count) * (i + 1 / m_count)));
            }
        }

        private Image CreatePicker(string namePicker, out TextMeshProUGUI pickerText)
        {
            GameObject picker = new GameObject(namePicker);
            picker.transform.parent = transform;
            picker.transform.localPosition = Vector3.zero;

            RectTransform pickerRect = picker.AddComponent<RectTransform>();
            pickerRect.anchorMin = Vector2.zero;
            pickerRect.anchorMax = Vector2.one;
            pickerRect.anchoredPosition = Vector2.zero;
            pickerRect.sizeDelta = Vector2.zero;

            if (m_textGroup == null)
            {
                m_textGroup = new GameObject("Text Picker Spin Wheel Group").AddComponent<RectTransform>();
                m_textGroup.transform.position = Vector3.zero;
                m_textGroup.transform.parent = transform.parent;
                m_textGroup.transform.SetSiblingIndex(99);
                m_textGroup.transform.localPosition = Vector3.zero;
                m_textGroup.anchorMin = Vector2.one * 0.5f;
                m_textGroup.anchorMax = Vector2.one * 0.5f;
                m_textGroup.sizeDelta = new Vector2(200, 200);
                m_textGroup.anchoredPosition = Vector2.zero;
            }
            TextMeshProUGUI pickerNameText = new GameObject("Picker Name Text").AddComponent<TextMeshProUGUI>();
            pickerNameText.text = namePicker;
            pickerNameText.font = m_fontAsset;
            pickerNameText.fontStyle = m_fontStyles;
            pickerNameText.fontSize = m_fontSize;
            pickerNameText.color = m_fontColor;
            pickerNameText.alignment = m_textAlignment;
            
            pickerNameText.rectTransform.anchorMin = new Vector2(0, 0.5f);
            pickerNameText.rectTransform.anchorMax = new Vector2(0, 0.5f);
            pickerNameText.rectTransform.pivot = new Vector2(0, 0.5f);
            pickerNameText.transform.parent = m_textGroup.transform;
            pickerNameText.transform.position = Vector3.zero;
            pickerNameText.rectTransform.anchoredPosition = new Vector2(100, 0);
            pickerNameText.rectTransform.sizeDelta = new Vector2(100, 200);
            m_pickerText.Add(pickerNameText.text);
            pickerText = pickerNameText;

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
            if (GUI.Button(new Rect(20, 20, 150, 50), "Create Spin Wheel"))
                CreatSpinWheel();

            if (GUI.Button(new Rect(20, 90, 150, 50), "Play Spin"))
            {
                GetComponent<Animator>().enabled = true;
                GetComponent<Animator>().SetBool("IsReset", false);
            }
            
            if (GUI.Button(new Rect(20, 160, 150, 50), "Reset Spin"))
                GetComponent<Animator>().SetBool("IsReset", true);
        }
    }
}