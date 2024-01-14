using UnityEngine;

namespace GDD
{
    public class TriggerHandleLayer : TriggerHandle<int>
    {
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            m_triggerEnterEvent?.Invoke(other.gameObject.layer);
        }

        protected override void OnTriggerStay(Collider other)
        {
            base.OnTriggerStay(other);
            m_triggerStayEvent?.Invoke(other.gameObject.layer);
        }

        protected override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
            m_triggerExitEvent?.Invoke(other.gameObject.layer);
        }
    }
}