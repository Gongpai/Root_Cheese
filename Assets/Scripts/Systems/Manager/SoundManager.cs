using GDD.Sinagleton;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public class SoundManager : CanDestroy_Sinagleton<SoundManager>
    {
        [SerializeField] private UnityEvent m_onFadeOutAllSound;
        public UnityEvent onFadeOutAllSound
        {
            get => m_onFadeOutAllSound;
            set => m_onFadeOutAllSound = value;
        }
        
        public void FadeOutAllSound()
        {
            m_onFadeOutAllSound?.Invoke();
        }
    }
}