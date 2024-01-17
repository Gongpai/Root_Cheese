using System;
using System.Collections.Generic;
using GDD.Timer;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using FontStyles = TMPro.FontStyles;
using Random = UnityEngine.Random;

namespace GDD
{
    public class SpinWheelUI : MonoBehaviour
    {
        [Header("Text")] 
        [SerializeField] protected TMP_FontAsset m_fontAsset;
        [SerializeField] protected FontStyles m_fontStyles;
        [SerializeField] protected FontWeight m_fontWeight;
        [SerializeField] protected float m_fontSize;
        [SerializeField] protected Color m_fontColor;
        [SerializeField] protected TextAlignmentOptions m_textAlignment;
        [SerializeField] protected float m_line = -60;
        [SerializeField] protected Vector2 textSize = new Vector2(100, 200);

        [Header("Picker")] 
        [SerializeField] protected GameObject m_pickerResult;
        [SerializeField] protected TextMeshProUGUI m_pickerTextResult;
        [SerializeField] protected UnityEvent m_OnEnd;
        [SerializeField] protected int m_count;
        protected Image m_image;
        protected RectTransform _rectTransform;
        protected float _randomRot;
        protected bool _isShowResult;
        protected RectTransform m_textGroup;
        protected List<string> m_pickerText = new List<string>();

        public float rotationZ;

        protected virtual void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            
            CreateSpinWheel();
            GetComponent<Animator>().enabled = true;

            AwaitTimer timer = new AwaitTimer(10.0f, () =>
            {
                m_pickerResult.GetComponent<Animator>().SetBool("Revert", true);
                m_OnEnd?.Invoke();
            }, time =>
            {
                print($"Time = {time}");
            });

            timer.Start();
        }

        protected virtual void Update()
        {
            _rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 1) * (rotationZ * _randomRot));
            
            if(m_textGroup != null)
                m_textGroup.rotation = Quaternion.Euler(new Vector3(0, 0, 1) * (rotationZ * _randomRot));
            
            if(rotationZ >= 1.0f)
            {
                _isShowResult = true;
                
                if(!m_pickerResult.activeSelf)
                    ShowResult();
                
                m_pickerResult.SetActive(true);
            }
        }

        protected virtual void ShowResult()
        {
            float indexRot = m_count + (_randomRot) % -360 / (360 / m_count);
            print($"Random : {_randomRot}");
            print($"Index : {indexRot}");
            m_pickerTextResult.text = m_pickerText[Mathf.FloorToInt(indexRot)];
        }
        
        protected virtual void CreateSpinWheel()
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
                Image picker = CreatePicker(GetPickerName(i), out TextMeshProUGUI _pickerText);
                picker.fillAmount = (degree / m_count) / degree;
                _pickerText.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, (degree / m_count) * -i)  - new Vector3(0, 0, (degree / m_count) / 2 - 90));
                
                print($"Rotation : {new Vector3(0, 0, (degree / m_count) * -i)  - new Vector3(0, 0, (degree / m_count) / 2)}");
                picker.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, (degree / m_count) * (i + 1 / m_count)));
            }
        }

        protected virtual string GetPickerName(int i = 0)
        {
            return $"Picker {i + 1}";
        }

        protected virtual Image CreatePicker(string namePicker, out TextMeshProUGUI pickerText)
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
                m_textGroup.transform.SetSiblingIndex(m_pickerResult.transform.GetSiblingIndex() - 1);
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
            pickerNameText.lineSpacing = m_line;
            pickerNameText.color = m_fontColor;
            pickerNameText.alignment = m_textAlignment;
            
            pickerNameText.rectTransform.anchorMin = new Vector2(0, 0.5f);
            pickerNameText.rectTransform.anchorMax = new Vector2(0, 0.5f);
            pickerNameText.rectTransform.pivot = new Vector2(-0.38f, 0.5f);
            pickerNameText.transform.parent = m_textGroup.transform;
            pickerNameText.transform.position = Vector3.zero;
            pickerNameText.rectTransform.anchoredPosition = new Vector2(100, 0);
            pickerNameText.rectTransform.sizeDelta = textSize;
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

        protected virtual void OnGUI()
        {
            /*
            if (GUI.Button(new Rect(20, 20, 150, 50), "Create Spin Wheel"))
                CreatSpinWheel();

            if (GUI.Button(new Rect(20, 90, 150, 50), "Play Spin"))
            {
                GetComponent<Animator>().enabled = true;
                GetComponent<Animator>().SetBool("IsReset", false);
            }
            
            if (GUI.Button(new Rect(20, 160, 150, 50), "Reset Spin"))
                GetComponent<Animator>().SetBool("IsReset", true);
                */
        }
    }
}