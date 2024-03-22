using System;
using System.Collections;
using UnityEngine;

namespace GDD
{
    public class AutoFadeOutSound : MonoBehaviour
    {
        [SerializeField] private float duration;
        private float _currentTime;
        private SoundManager SM;
        private AudioSource _audioSource;
        private bool isStart;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _currentTime = duration;
        }

        private void OnEnable()
        {
            SM = SoundManager.Instance;
            SM.onFadeOutAllSound.AddListener(StartFadeOut);
        }

        private void Update()
        {
            if(!isStart) 
                return;
            else
            {
                StartCoroutine(FadeSound());
                isStart = false;
            }
            
        }

        IEnumerator FadeSound()
        {
            while (_currentTime > 0)
            {
                _currentTime -= 0.01f;
                _audioSource.volume = _currentTime / duration;

                yield return new WaitForSecondsRealtime(0.01f);
            }
        }

        private void StartFadeOut()
        {
            isStart = true;
        }

        private void OnDisable()
        {
            SM.onFadeOutAllSound.RemoveListener(StartFadeOut);
        }
    }
}