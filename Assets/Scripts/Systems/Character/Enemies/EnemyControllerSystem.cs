using UnityEngine;
using UnityEngine.AI;

namespace GDD
{
    public class EnemyControllerSystem : CharacterControllerSystem<EnemySystem>
    {
        // AI variables
        [Header("AI Setting")]
        [Tooltip("Sprint speed of the AI in m/s, controls NavMeshAgent's speed")]
        public float SprintSpeed = 5.335f;
        [Tooltip("Target destination for Nav Mesh Agent as Transform")]
        public Transform Target;
        [Tooltip("If the AI is sprinting or not.")]
        public bool Sprinting = false;
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;
        [Tooltip("If the AI will start a Jump or not.")]
        public bool Jump = false;

        protected NavMeshAgent thisAgent;

        protected override void Start()
        {
            base.Start();
            
            thisAgent = GetComponent<NavMeshAgent>();
            thisAgent.updateRotation = false;
            
            if(Sprinting) thisAgent.speed = SprintSpeed;
            else thisAgent.speed = MoveSpeed;
            
            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        protected override void Update()
        {
            if (Target != null)
            {
                thisAgent.SetDestination(Target.position);
            }
            
            SimulateGravity();
            GroundedCheck();
            
            
            
            if (Sprinting) thisAgent.speed = SprintSpeed;
            else thisAgent.speed = MoveSpeed;

            if (thisAgent.remainingDistance > thisAgent.stoppingDistance)
                this.Move(thisAgent.desiredVelocity.normalized, thisAgent.desiredVelocity.magnitude);
            else
	            this.Move(thisAgent.desiredVelocity.normalized, 0f);
        }
        
        public virtual void Move(Vector3 AgentDestination)
        {
	        this.Move(AgentDestination,_speed);
        }

        public virtual float Move(Vector3 AgentDestination, float AgentSpeed)
        {
	        if (AgentSpeed > 0f)
	        {
		        // a reference to the players current horizontal velocity
		        float currentHorizontalSpeed =
			        new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

		        float speedOffset = 0.1f;

		        // accelerate or decelerate to target speed
		        if (currentHorizontalSpeed < AgentSpeed - speedOffset ||
		            currentHorizontalSpeed > AgentSpeed + speedOffset)
		        {
			        // creates curved result rather than a linear one giving a more organic speed change
			        // note T in Lerp is clamped, so we don't need to clamp our speed
			        _speed = Mathf.Lerp(currentHorizontalSpeed, AgentSpeed, Time.deltaTime * SpeedChangeRate);

			        // round speed to 3 decimal places
			        _speed = Mathf.Round(_speed * 1000f) / 1000f;
		        }
		        else
		        {
			        _speed = AgentSpeed;
		        }

		        _animationBlend = Mathf.Lerp(_animationBlend, AgentSpeed, Time.deltaTime * SpeedChangeRate);

		        // rotate player when the player is moving
		        if (_speed != 0f)
		        {
			        _targetRotation = Mathf.Atan2(AgentDestination.x, AgentDestination.z) * Mathf.Rad2Deg;
			        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation,
				        ref _rotationVelocity, RotationSmoothTime);
			        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
		        }

		        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

		        // move the player
		        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
		                         new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

		        // update animator if using character
		        float theMagnitude = 1f;
		        _animator.SetFloat(_animIDSpeed, _animationBlend);
		        _animator.SetFloat(_animIDMotionSpeed, theMagnitude);
		        return theMagnitude;

	        }
	        else
	        {
		        _animationBlend = Mathf.Lerp(_animationBlend, 0f, Time.deltaTime * SpeedChangeRate);
		        _animator.SetFloat(_animIDSpeed, _animationBlend);
		        _animator.SetFloat(_animIDMotionSpeed, 1f);
		        return 1;
	        }
        }

        protected override void SimulateGravity()
        {
	        if (Grounded)
	        {
		        // reset the fall timeout timer
		        _fallTimeoutDelta = FallTimeout;

		        // update animator
		        _animator.SetBool(_animIDJump, false);
		        _animator.SetBool(_animIDFreeFall, false);

		        // stop our velocity dropping infinitely when grounded
		        if (_verticalVelocity < 0.0f)
		        {
			        _verticalVelocity = -2f;
		        }

		        // Jump
		        if (Jump && _jumpTimeoutDelta <= 0.0f)
		        {
			        // the square root of H * -2 * G = how much velocity needed to reach desired height
			        _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

			        // update animator if using character
			        _animator.SetBool(_animIDJump, true);
		        }

		        // jump timeout
		        if (_jumpTimeoutDelta >= 0.0f)
		        {
			        _jumpTimeoutDelta -= Time.deltaTime;
		        }
	        }
	        else
	        {
		        // reset the jump timeout timer
		        _jumpTimeoutDelta = JumpTimeout;

		        // fall timeout
		        if (_fallTimeoutDelta >= 0.0f)
		        {
			        _fallTimeoutDelta -= Time.deltaTime;
		        }
		        else
		        {
			        // update animator if using character
			        _animator.SetBool(_animIDFreeFall, true);
		        }

		        // if we are not grounded, do not jump
		        Jump = false;
	        }

	        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
	        if (_verticalVelocity < _terminalVelocity)
	        {
		        _verticalVelocity += Gravity * Time.deltaTime;
	        }

	        // Trigger Jump Once
	        Jump = false;
        }
    }
}