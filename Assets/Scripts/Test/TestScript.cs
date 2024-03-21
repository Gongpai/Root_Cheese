using System;
using System.Collections.Generic;
using GDD.Timer;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Test
{
    public class TestScript : MonoBehaviour
    {
       [SerializeField] private List<MonoBehaviour> _scripts;
        private float _currentTime = 0f;
        private AwaitTimer timer;
        private TestScript2 _script2;
        private float distance = 1000;
        private Transform target;
        private void Start()
        {
            _script2 = TestScript2.Instance;

            foreach (var script in _scripts)
            {
                script.enabled = false;
            }
            
            /*timer = new AwaitTimer(2, () =>
                {
                    print("Enddddddddddd--------------------------------------");
                },
                (time =>
                {
                    print($"Time is = {time}");
                }));*/
        }

        private void Update()
        {
            
            float currentDistance = 1000;
            
            foreach (var playerTransform in _script2.m_players)
            {
                float playerDistance = Vector3.Distance(transform.position, playerTransform.position);

                if (playerDistance < currentDistance)
                {
                    currentDistance = playerDistance;
                    
                    if(target != null)
                        target.GetComponent<TestScript3>().m_UI.SetActive(false);
                    target = playerTransform;
                }
            }
            
            distance = currentDistance;
            target.GetComponent<TestScript3>().m_UI.SetActive(true);
            print($"Distance : {distance} || Name is : {target.name}");
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