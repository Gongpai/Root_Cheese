using System;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class PlaySoundsLoop : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> m_audioClips;
        private AudioSource _audioSource;
        private int _audioIndex = 0;
        private bool isPlayEnd;
        private bool isEndOfClips;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.clip = m_audioClips[0];
            _audioSource.Play();
        }

        private void Update()
        {
            if (isEndOfClips)
                return;
            
            print($"PlayBack Time : {_audioSource.time} / {m_audioClips[_audioIndex].length}");
            if (_audioSource.time >= m_audioClips[_audioIndex].length - Time.deltaTime)
            {
                print($"Old Index : {_audioIndex}");
                _audioIndex++;

                if (_audioIndex >= m_audioClips.Count - 1)
                {
                    _audioIndex = m_audioClips.Count - 1;
                    _audioSource.loop = true;
                    isEndOfClips = true;
                }

                _audioSource.clip = m_audioClips[_audioIndex];
                _audioSource.Play();
            }
        }
    }
}