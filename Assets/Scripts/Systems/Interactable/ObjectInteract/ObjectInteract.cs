using System;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public class ObjectInteract : MonoBehaviour, IInteract
    {
        [SerializeField] private UnityEvent m_interactEvent;
        
        public void OnInteract()
        {
            m_interactEvent?.Invoke();
        }
    }
}