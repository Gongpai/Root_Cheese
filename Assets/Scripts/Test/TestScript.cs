using System;
using GDD.Timer;
using UnityEngine;

namespace Test
{
    public class TestScript : MonoBehaviour
    {
        private float _currentTime = 0f;
        
        private void Start()
        {
            AwaitTimer timer = new AwaitTimer(2, () =>
                {
                    print("Enddddddddddd");
                },
                (time =>
                {
                    print($"Time is = {time}");
                }));

            timer.Start();
        }

        private void Update()
        {
            if (_currentTime < 2.0f)
            {
                _currentTime += Time.deltaTime;
                print($"Time update is = {_currentTime}");
            }
        }
    }
}