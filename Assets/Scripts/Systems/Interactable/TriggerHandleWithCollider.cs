using UnityEngine;

namespace GDD
{
    public class TriggerHandleWithCollider : TriggerHandle<Collider>
    {
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            
            m_triggerEnterEvent?.Invoke(other);
        }

        protected override void OnTriggerStay(Collider other)
        {
            base.OnTriggerStay(other);
            
            m_triggerStayEvent?.Invoke(other);
        }

        protected override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
            
            m_triggerExitEvent?.Invoke(other);
        }
    }
}