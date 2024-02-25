using System;
using System.Collections;
using System.Threading;
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
        private Task _task;
        private CancellationTokenSource cts;
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
            isStart = true;
            
            cts = new CancellationTokenSource();
            
            _task = Timer(cts.Token);
            await _task;
        }
        
        public void Stop()
        {
            //Debug.LogWarning("Timer is Stop!!!!!");
            isStop = true;
            isStart = false;
            
            if(cts != null)
                cts.Cancel();
            
            cts = new CancellationTokenSource();
        }
        
        public async Task Timer(CancellationToken ct)
        {
            float _currentTime;
            _isRunning = true;
            //Debug.LogWarning("Timer is Start!!!!!");

            try
            {
                _currentTime = 0;
                while (_currentTime <= _time)
                {
                    _currentTime += Time.deltaTime;
                    //Debug.LogWarning($"Invoke : {_time}");
                    _actionElapsed?.Invoke(_currentTime);
                    await Task.Delay(_delayTime);
                    ct.ThrowIfCancellationRequested();

                    if (!Application.isPlaying)
                    {
                        _isRunning = false;
                        _actionEnd?.Invoke();
                       // Debug.LogWarning("Timer is Stop {App Not Playing}");
                        return;
                    }
                
                    if (isStop)
                    {
                        _isRunning = false;
                        //Debug.LogWarning("Timer is Stop {Pause Timer}");
                        return;
                    }
                }

                _isRunning = false;
                _actionEnd?.Invoke();
                Stop();
            }
            catch (OperationCanceledException e)
            {
                Debug.Log(e);
                throw;
            }
        }
    }
}