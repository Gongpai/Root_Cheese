using System;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public class TriggerHandle<T> : MonoBehaviour
    {
        [Header("ID")] 
        [SerializeField] protected T checkID;
        
        [Header("On Trigger Enter")] 
        [SerializeField] protected UnityEvent<T> m_triggerEnterEvent;
        
        [Header("On Trigger Stay")] 
        [SerializeField] protected UnityEvent<T> m_triggerStayEvent;
        
        [Header("On Trigger Exit")] 
        [SerializeField] protected UnityEvent<T> m_triggerExitEvent;

        protected virtual void OnTriggerEnter(Collider other)
        {
            
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            
        }
    }
}