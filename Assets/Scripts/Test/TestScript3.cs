using System;
using TMPro;
using UnityEngine;

namespace Test
{
    public class TestScript3 : MonoBehaviour
    {
        public GameObject m_UI;

        private void Start()
        {
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = gameObject.name;
        }
    }
}