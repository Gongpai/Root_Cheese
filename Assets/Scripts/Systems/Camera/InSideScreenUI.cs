using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public class InSideScreenUI : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private Transform point;
        [SerializeField] private Vector2 m_border;
        [SerializeField] private Vector2 m_offsetHorizontal;
        [SerializeField] private Vector2 m_offsetVertical;

        [Header("Events")] 
        [SerializeField] private UnityEvent m_OnOverX;
        [SerializeField] private UnityEvent m_OnUnderX;
        [SerializeField] private UnityEvent m_OnInSideX;
        [SerializeField] private UnityEvent m_OnOverY;
        [SerializeField] private UnityEvent m_OnUnderY;
        [SerializeField] private UnityEvent m_OnInSideY;

        private bool isOverX;
        private bool isUnderX;
        private bool isOverY;
        private bool isUnderY;
        private bool isInSideX;
        private bool isInSideY;
        private RectTransform target;
        private Vector2 sizeUI;
        private Vector2 halfSizeUI;

        public Vector2 border
        {
            get => m_border;
            set => m_border = value;
        }

        public Vector2 offsetHorizontal
        {
            get => m_offsetHorizontal;
            set => m_offsetHorizontal = value;
        }

        public Vector2 offsetVertical
        {
            get => m_offsetVertical;
            set => m_offsetVertical = value;
        }

        private void Start()
        {
            target = GetComponent<RectTransform>();
            
            if(point == null)
                point = transform.parent.parent;
        }

        private void Update()
        {
            sizeUI = target.sizeDelta;
            halfSizeUI = sizeUI / 2;
            Vector3 Point = Camera.main.WorldToScreenPoint(point.position);
            Vector2 ScreenResolution = new Vector2(Screen.width, Screen.height);
            Vector2 screenPoint =  new Vector2(Point.x, Point.y - Screen.height);
            
            //X
            if (screenPoint.x > ScreenResolution.x - m_border.x - halfSizeUI.x - m_offsetHorizontal.y)
            {
                //print($"Lock Over X");
                target.anchoredPosition = new Vector2(ScreenResolution.x - m_border.x - halfSizeUI.x - m_offsetHorizontal.y, target.anchoredPosition.y);
                
                if(!isOverX)
                    m_OnOverX?.Invoke();

                isOverX = true;
                isInSideX = false;
            }
            else if (screenPoint.x < halfSizeUI.x + m_border.x + m_offsetHorizontal.x)
            {
                //print($"Lock Under X");
                target.anchoredPosition = new Vector2(halfSizeUI.x + m_border.x + m_offsetHorizontal.x, target.anchoredPosition.y);
                
                if(!isUnderX)
                    m_OnUnderX?.Invoke();

                isUnderX = true;
                isInSideX = false;
            }
            else
            {
                //print($"Pass X");
                target.anchoredPosition = new Vector2(screenPoint.x, target.anchoredPosition.y);
                
                if(!isInSideX)
                    m_OnInSideX?.Invoke();

                isInSideX = true;
                isOverX = false;
                isUnderX = false;
            }

            //Y
            if (screenPoint.y < -(ScreenResolution.y - halfSizeUI.y - m_border.y - m_offsetVertical.y))
            {
                //print($"Lock Over Y");
                target.anchoredPosition = new Vector2(target.anchoredPosition.x, -(ScreenResolution.y - halfSizeUI.y - m_border.y - m_offsetVertical.y));
                
                if(!isOverY)
                    m_OnOverY?.Invoke();
                
                isOverY = true;
                isInSideY = false;
            }
            else if (screenPoint.y > -(halfSizeUI.y + m_border.y + m_offsetVertical.x))
            {
                //print($"Lock Under Y");
                target.anchoredPosition = new Vector2(target.anchoredPosition.x, -(halfSizeUI.y + m_border.y  + m_offsetVertical.x));
                
                if(!isUnderY)
                    m_OnUnderY?.Invoke();
                
                isUnderY = true;
                isInSideY = false;
            }
            else
            {
                //print($"Pass Y");
                target.anchoredPosition = new Vector2(target.anchoredPosition.x, screenPoint.y);
                
                if(!isInSideY)
                    m_OnInSideY?.Invoke();
                
                isInSideY = true;
                isOverY = false;
                isUnderY = false;
            }
        }
        
        public void SetBorderX(float value)
        {
            m_border = new Vector2(value, m_border.y);
        }
        
        public void SetBorderY(float value)
        {
            m_border = new Vector2(m_border.x, value);
        }

        public void SetOffsetHorizontalX(float value)
        {
            m_offsetHorizontal = new Vector2(value, m_offsetHorizontal.y);
        }
        
        public void SetOffsetHorizontalY(float value)
        {
            m_offsetHorizontal = new Vector2(m_offsetHorizontal.x, value);
        }

        public void  SetOffsetVerticalX(float value)
        {
            m_offsetVertical = new Vector2(value, m_offsetVertical.y);
        }
        
        public void  SetOffsetVerticalY(float value)
        {
            m_offsetVertical = new Vector2(m_offsetVertical.x, value);
        }
    }
}