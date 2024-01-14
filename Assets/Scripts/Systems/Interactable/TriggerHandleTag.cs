using UnityEngine;

namespace GDD
{
    public class TriggerHandleTag : TriggerHandle<string>
    {
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            m_triggerEnterEvent?.Invoke(other.tag);
        }

        protected override void OnTriggerStay(Collider other)
        {
            base.OnTriggerStay(other);
            m_triggerStayEvent?.Invoke(other.tag);
        }

        protected override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
            m_triggerExitEvent?.Invoke(other.tag);
        }
    }
}