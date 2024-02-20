using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace GDD.Timer
{
    public class AwaitTimeCounting
    {
        private float _time;
        private int _delayTime = 1;
        private UnityAction<float> _counting;
        private UnityAction _stopCounting;
        private Task _task;
        private CancellationTokenSource cts;
        private bool isStart = false;
        private bool _isRunning;

        public AwaitTimeCounting(UnityAction<float> Counting, UnityAction stopCounting)
        {
            _counting = Counting;
            _stopCounting = stopCounting;
        }

        public async Task Start()
        {
            isStart = true;
            
            if (cts != null)
                cts.Cancel();
            
            cts = new CancellationTokenSource();

            _task = Counting(cts.Token);
            await _task;
        }

        public async Task Counting(CancellationToken ct)
        {
            float _currentTime = 0;
            _isRunning = true;

            try
            {
                while (isStart)
                {
                    //Counting
                    _currentTime += Time.deltaTime;
                    _counting?.Invoke(_currentTime);

                    //Await
                    await Task.Delay(_delayTime);
                    ct.ThrowIfCancellationRequested();
                }
                
                _stopCounting?.Invoke();
                Stop();
            }
            catch (OperationCanceledException e)
            {
                Debug.Log(e);
                throw;
            }
        }
        
        public void Stop()
        {
            isStart = false;
            
            if(cts != null)
                cts.Cancel();
        }
    }
}