using System;
using GDD.Timer;
using UnityEngine;
using UnityEngine.UI;

namespace Test
{
    public class TestScript : MonoBehaviour
    {
        private float _currentTime = 0f;
        private AwaitTimer timer;
        
        private void Start()
        {
            timer = new AwaitTimer(2, () =>
                {
                    print("Enddddddddddd--------------------------------------");
                },
                (time =>
                {
                    print($"Time is = {time}");
                }));
        }

        private void Update()
        {
            /*
            if (_currentTime < 2.0f)
            {
                _currentTime += Time.deltaTime;
                print($"Time update is = {_currentTime}");
            }
            */
        }

        private void OnGUI()
        {
            if(GUI.Button(new Rect(20,20, 250,75), "Start Timer"))
                timer.Start();
            
            if(GUI.Button(new Rect(20,115, 250,75), "Stop Timer"))
                timer.Stop();
            
        }
    }
}