using System;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private UnityEvent m_OnUIClose;
        [SerializeField] private bool _autoCloseUI = true;
        [SerializeField] private bool isOnlyPlayAnimation = true;
        private Animator _animator;
        private string _currentPlay;
        private string _currentStop;
        private bool _isPlay = false;
        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void PlayWithBool(string parameterName)
        {
            if(_currentStop != null)
                _animator.SetBool(_currentStop, true);
            _currentStop = null;
            
            if (parameterName == "")
                parameterName = "Play";

            if(_isPlay && !isOnlyPlayAnimation)
                return;
            
            _isPlay = true;
            _currentPlay = parameterName;
            _animator.SetBool(parameterName, true);
        }
        
        public void PlayWithTrigger(string parameterName)
        {
            if (parameterName == "")
                parameterName = "Play";
            
            if(_isPlay && !isOnlyPlayAnimation)
                return;
            
            _isPlay = true;
            _animator.SetTrigger(parameterName);
            if(_currentStop != null)
                _animator.SetBool(_currentStop, true);
        }

        public void RewindWithBool(string parameterName)
        {
            if(_currentPlay != null)
                _animator.SetBool(_currentPlay, false);
            _currentPlay = null;

            if (parameterName == "")
                parameterName = "Rewind";
            
            if(!_isPlay && !isOnlyPlayAnimation)
                return;
            
            _isPlay = false;
            _currentStop = parameterName;
            _animator.SetBool(parameterName, true);
        }
        
        public void RewindWithTrigger(string parameterName)
        {
            if (parameterName == "")
                parameterName = "Rewind";
            
            if(!_isPlay && !isOnlyPlayAnimation)
                return;
            
            _isPlay = false;
            _animator.SetTrigger(parameterName);
            if(_currentStop != null)
                _animator.SetBool(_currentStop, true);
        }

        public void CloseUI()
        {
            if(_autoCloseUI)
                gameObject.SetActive(false);
            m_OnUIClose?.Invoke();
        }
    }
}