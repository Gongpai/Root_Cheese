using UnityEngine;

namespace GDD
{
    public class TriggerHandleTag : TriggerHandle<string>
    {
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            
            if(checkID == "")
                m_triggerEnterEvent?.Invoke(other.tag);
            else if (other.tag == checkID)
                m_triggerEnterEvent?.Invoke(other.tag);
        }

        protected override void OnTriggerStay(Collider other)
        {
            base.OnTriggerStay(other);
            
            if(checkID == "")
                m_triggerStayEvent?.Invoke(other.tag);
            else if (other.tag == checkID)
                m_triggerStayEvent?.Invoke(other.tag);
        }

        protected override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
            
            if(checkID == "")
                m_triggerExitEvent?.Invoke(other.tag);
            else if (other.tag == checkID)
                m_triggerExitEvent?.Invoke(other.tag);
        }
    }
}