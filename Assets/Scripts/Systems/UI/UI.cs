using System;
using UnityEngine;

namespace GDD
{
    public class UI : MonoBehaviour
    {
        private SoundManager SM;

        protected virtual void Start()
        {
            SM = SoundManager.Instance;
        }

        public void FadeOutAllSound()
        {
            SM.FadeOutAllSound();
        }
        
        public virtual void Quit()
        {
            Application.Quit();
        }
    }
}