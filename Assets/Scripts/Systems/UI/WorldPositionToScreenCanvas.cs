using System;
using UnityEngine;

namespace GDD
{
    public class WorldPositionToScreenCanvas : MonoBehaviour
    {
        [SerializeField] private Transform point;
        private RectTransform target;

        private void Start()
        {
            target = GetComponent<RectTransform>();
            
            if(point == null)
                point = transform.parent.parent;
        }

        private void Update()
        {
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(point.position);
            //target.position = screenPoint;
            target.anchoredPosition = new Vector2(screenPoint.x, screenPoint.y - Screen.height);
        }
    }
}