using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace GDD.Timer
{
    public class AwaitTimer
    {
        private float _time;
        private int _delayTime = 1;
        private UnityAction _actionEnd;
        private UnityAction<float> _actionElapsed;
        private bool isStop = false;
        private bool isStart = false;
        private bool _isRunning;

        public int delayMillisecond
        {
            set => _delayTime = value;
        }

        public bool isRunning
        {
            get => _isRunning;
        }
        
        public AwaitTimer(float time, UnityAction actionEnd, UnityAction<float> actionElapsed = default)
        {
            _time = time;
            _actionEnd = actionEnd;
            _actionElapsed = actionElapsed;
        }

        public async Task Start()
        {
            isStop = false;
            if(!isStart)
                await Timer();
        }
        
        public async Task Timer()
        {
            float _currentTime = 0;
            isStart = true;
            _isRunning = true;
            
            while (_currentTime <= _time)
            {
                _currentTime += Time.deltaTime;
                _actionElapsed?.Invoke(_currentTime);
                await Task.Delay(_delayTime);

                if (!Application.isPlaying)
                {
                    _isRunning = false;
                    _actionEnd?.Invoke();
                    return;
                }
                
                if (isStop)
                {
                    _isRunning = false;
                    _actionEnd?.Invoke();
                    return;
                }
            }

            _isRunning = false;
            _actionEnd?.Invoke();
            Stop();
        }

        public void Stop()
        {
            isStop = true;
            isStart = false;
        }
    }
}