using System;
using GDD.ObjectPool;
using UnityEngine;

namespace GDD
{
    public class Target_Point : GameObjectPool
    {
        private float time = 0;
        private float max_time = 0;
        private bool is_start = false;

        private void Update()
        {
            if (is_start)
            {
                if (time >= max_time)
                {
                    ReturnToPool();
                }
                else
                {
                    time += Time.deltaTime;
                }
            }
        }

        public void OnStart(float maxtime)
        {
            max_time = maxtime;
            time = 0;
            is_start = true;
        }

        private void OnDisable()
        {
            is_start = false;
        }
    }
}