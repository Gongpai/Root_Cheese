using UnityEngine;

namespace GDD
{
    public class TakeDamageIncessant : TakeDamage
    {
        [SerializeField] protected float _currentTime = 0;
        [SerializeField] protected float _timeCount = 1;

        public float timeCount
        {
            set => _timeCount = value;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _currentTime = 0;
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            
        }

        protected override void OnTriggerStay(Collider other)
        {
            base.OnTriggerStay(other);
            
            if (_currentTime < _timeCount)
                _currentTime += Time.deltaTime;
            else
            {
                _currentTime = 0;
                OnTakeDamage(other);
                //print("Take HPHPHPHPHPH");
            }
        }

        public override void ReturnToPool()
        {
            
        }

        public void ReturnBulletToPool()
        {
            
        }
    }
}