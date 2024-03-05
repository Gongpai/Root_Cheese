using System;
using TMPro;
using UnityEngine;

namespace GDD
{
    public class SelectChapterSystem : MonoBehaviour
    {
        [Header("UI")] 
        [SerializeField] protected TextMeshProUGUI m_chapter;
        [SerializeField] protected MoveMultiObject m_moveMultiObject;
        protected GameManager GM;

        protected virtual void Start()
        {
            GM = GameManager.Instance;
            m_moveMultiObject.OnSelect.AddListener(SetChapter);
        }

        protected virtual void Update()
        {
            m_chapter.text = $"Chapter {GM.selectChapter + 1}";
        }

        protected virtual void SetChapter(int value)
        {
            GM.selectChapter = value;
        }
    }
}