using UnityEngine;

namespace GDD
{
    [RequireComponent(typeof(ProjectileLauncherCalculate))]
    [RequireComponent(typeof(ProjectileJumpState))]
    public class EnemyJumpState : EnemyAttackState
    {
        private ProjectileLauncherCalculate _PLC;
        private Vector3 _velocity;
        private ProjectileJumpState _projectileJumpState;
        private bool isLand;
        
        protected override void Start()
        {
            base.Start();
            _PLC = GetComponent<ProjectileLauncherCalculate>();
            _projectileJumpState = GetComponent<ProjectileJumpState>();
        }

        protected override void Update()
        {
            base.Update();
        }

        public override string StateName()
        {
            return "EnemyJumpState";
        }

        public override void OnStart(EnemySystem contrller)
        {
            base.OnStart(contrller);

            print("Enter Start");
            isLand = false;
            _velocity = _PLC.GetVelocityProjectile(transform.position, target.position, transform.position.y);
        }

        public override void Handle(EnemySystem contrller)
        {
            base.Handle(contrller);
            
            if(isLand)
                _projectileJumpState.OnExitState();
            else 
                transform.position = VelocityCalculate(transform.position, _velocity, Physics.gravity.magnitude, out isLand);
            
            print($"Is Land : {isLand}");
        }

        public override void OnExit()
        {
            base.OnExit();
            print("Enter Exit");
        }
        
        private Vector3 VelocityCalculate(Vector3 currentPosition, Vector3 currentVelocity, float gravity, out bool isLand)
        {
            float deltaTime = Time.deltaTime;

            // คำนวณตำแหน่งใหม่ของ projectile
            currentPosition.x += currentVelocity.x * deltaTime;
            currentPosition.y += currentVelocity.y * deltaTime - 0.5f * gravity * deltaTime * deltaTime;
            currentPosition.z += currentVelocity.z * deltaTime;

            // อัปเดตความเร็วในแนวแกน y โดยใช้การลดความเร็วจากแรงโน้มถ่วง
            currentVelocity.y -= gravity * deltaTime;

            _velocity = currentVelocity;
            isLand = currentVelocity.y <= 0;
            return currentPosition;
        }
    }
}