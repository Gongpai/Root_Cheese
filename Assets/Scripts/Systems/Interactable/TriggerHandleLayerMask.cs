using UnityEngine;

namespace GDD
{
    public class TriggerHandleLayerMask : TriggerHandle<LayerMask>
    {
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            
            if(checkID == ~0)
                m_triggerEnterEvent?.Invoke(other.gameObject.layer);
            else if (other.gameObject.layer == checkID)
                m_triggerEnterEvent?.Invoke(other.gameObject.layer);
        }

        protected override void OnTriggerStay(Collider other)
        {
            base.OnTriggerStay(other);
            
            if(checkID == ~0)
                m_triggerStayEvent?.Invoke(other.gameObject.layer);
            else if (other.gameObject.layer == checkID)
                m_triggerStayEvent?.Invoke(other.gameObject.layer);
        }

        protected override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
            
            if(checkID == ~0)
                m_triggerExitEvent?.Invoke(other.gameObject.layer);
            else if (other.gameObject.layer == checkID)
                m_triggerExitEvent?.Invoke(other.gameObject.layer);
        }
    }
}