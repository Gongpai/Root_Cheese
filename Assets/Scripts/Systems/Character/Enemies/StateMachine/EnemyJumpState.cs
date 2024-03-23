using System;
using Cinemachine.Utility;
using Photon.Pun;
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
        private MultiplayerEnemyController _multiplayerEnemyController;
        private GameObject _landVFX;
        private GameObject spawnLandVFX;
        private float _damage;
        private float oldPosJump;
        private bool isJump;
        private bool isLand;
        private Vector3 m_ColliderCenter;
        private float m_ColliderRadius;
        private bool m_isTigger;

        public GameObject landVFX
        {
            set => _landVFX = value;
        }

        public float damage
        {
            set => _damage = value;
        }

        public Vector3 ColliderCenter
        {
            set => m_ColliderCenter = value;
        }

        public float ColliderRadius
        {
            set => m_ColliderRadius  =value;
        }

        public bool isTigger
        {
            set => m_isTigger = value;
        }

        
        protected override void Start()
        {
            base.Start();
            _PLC = GetComponent<ProjectileLauncherCalculate>();
            _projectileJumpState = GetComponent<ProjectileJumpState>();
            _multiplayerEnemyController = GetComponent<MultiplayerEnemyController>();
            _isFreezeLookAt = true;
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

            isLand = false;
            //print("Enter Start");
            _velocity = _PLC.GetVelocityProjectile(transform.position, target.position, transform.position.y);
            oldPosJump = transform.position.y;
            isJump = false;
        }

        public override void Handle(EnemySystem contrller)
        {
            base.Handle(contrller);
            //print("Enter Handle");
            
            if(isLand || _velocity.IsNaN())
                _projectileJumpState.OnExitState();
            else
            {
                transform.position = VelocityCalculate(transform.position, _velocity, Physics.gravity.magnitude, out isLand);
                //print($"Is Land : {isLand} | {_velocity} | {transform.position}");
            }
        }

        public override void OnExit()
        {
            if (spawnLandVFX == null)
                CreateLandVFX();
            else
                spawnLandVFX.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            
            spawnLandVFX.SetActive(true);
            
            base.OnExit();
        }

        public void CreateLandVFX()
        {
            //Add & Set TakeExplosiveDamage Component
            spawnLandVFX = Instantiate(_landVFX, new Vector3(transform.position.x, 0, transform.position.z), transform.rotation);
            spawnLandVFX.transform.localScale = Vector3.one;
            TakeDamage _takeDamage = spawnLandVFX.AddComponent<TakeDamage>();
            _takeDamage.ownerLayer = transform;
            _takeDamage.OwnerViewID = GetComponent<PhotonView>().ViewID;
            _takeDamage.damage = _damage;
                
            //Add & Set SphereCollider Component
            SphereCollider _sphereCollider = spawnLandVFX.AddComponent<SphereCollider>();
            _sphereCollider.center = m_ColliderCenter;
            _sphereCollider.radius = m_ColliderRadius;
            _sphereCollider.isTrigger = m_isTigger;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            if(spawnLandVFX != null && spawnLandVFX.activeSelf)
                Gizmos.DrawWireSphere(spawnLandVFX.transform.position, m_ColliderRadius);
        }
        
        private Vector3 VelocityCalculate(Vector3 currentPosition, Vector3 currentVelocity, float gravity, out bool isLand)
        {
            float deltaTime = Time.deltaTime;
            // New Position projectile
            currentPosition.x += currentVelocity.x * deltaTime;
            currentPosition.y += currentVelocity.y * deltaTime - 0.5f * gravity * deltaTime * deltaTime;
            currentPosition.z += currentVelocity.z * deltaTime;

            // Update Y Velocity
            Vector3? newVelocity = new Vector3(currentVelocity.x, currentVelocity.y -= gravity * deltaTime, currentVelocity.z);
            _velocity = newVelocity.Value;

            isLand = oldPosJump > currentPosition.y ? _multiplayerEnemyController.isGrounded : false;
            oldPosJump = currentPosition.y;
            return currentPosition;
        }

        public override void ApplyEnemyStrategy()
        {
            
        }

        public override void WithdrawEnemyStrategy()
        {
            
        }
    }
}