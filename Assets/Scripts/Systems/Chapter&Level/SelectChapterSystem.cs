using System;
using UnityEngine;

namespace GDD
{
    public class SelectChapterSystem : MonoBehaviour
    {
        [SerializeField] private MoveMultiObject m_moveMultiObject;
        private GameManager GM;

        private void Start()
        {
            GM = GameManager.Instance;
        }

        private void Update()
        {
            GM.selectChapter = m_moveMultiObject.select + 1;
        }
    }
}