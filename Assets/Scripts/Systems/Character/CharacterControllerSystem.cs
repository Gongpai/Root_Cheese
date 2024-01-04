using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace GDD
{
    [RequireComponent(typeof(PlayerInput))]
    public class CharacterControllerSystem : MonoBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        [SerializeField]protected float MoveSpeed = 2.0f;
        
        [Tooltip("Acceleration and deceleration")]
        [SerializeField]protected float SpeedChangeRate = 10.0f;
        
        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        [SerializeField]protected float RotationSmoothTime = 0.12f;
        
        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        [SerializeField]protected float Gravity = -15.0f;
        
        [Tooltip("What layers the character uses as ground")]
        [SerializeField]protected LayerMask GroundLayers;
        
        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        [SerializeField]protected bool Grounded = true;
        
        [Tooltip("Useful for rough ground")]
        [SerializeField]protected float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        [SerializeField]protected float GroundedRadius = 0.28f;
        
        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        [SerializeField]protected float FallTimeout = 0.15f;

        [Header("Audio")]
        [SerializeField] protected CharacterFootStepAudioClipLists _footStepAudioClipLists;
        
        // player
        protected float _speed;
        protected float _animationBlend;
        protected float _targetRotation = 0.0f;
        protected float _rotationVelocity;
        protected float _verticalVelocity;
        protected float _terminalVelocity = 53.0f;
        [SerializeField]protected bool _isCharacterMove = false;
        
        // timeout deltatime
        protected float _jumpTimeoutDelta;
        protected float _fallTimeoutDelta;
        
        // animation IDs
        protected int _animIDSpeed;
        protected int _animIDMotionSpeed;
        protected int _animIDGrounded;
        protected int _animIDJump;
        protected int _animIDFreeFall;
        
        // input system
        protected PlayerInput _playerInput;
        protected Animator _animator;
        
        // Other
        protected CharacterController _controller;
        protected AssetsInputsSystem _input;
        protected bool _hasAnimator;
        protected GameObject _mainCamera;
        
        //Player State
        public bool Get_Player_Move
        {
            get => _isCharacterMove;
        }

        protected virtual void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }
        
        protected virtual void Start()
        {
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<AssetsInputsSystem>();
            _playerInput = GetComponent<PlayerInput>();
            
            AssignAnimationIDs();
        }

        protected virtual void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);
            
            SimulateGravity();
            GroundedCheck();
            Move();
        }

        protected void AssignAnimationIDs()
        {
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }
        
        protected virtual void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f && this.enabled)
            {
                if (_footStepAudioClipLists.FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, _footStepAudioClipLists.FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(_footStepAudioClipLists.FootstepAudioClips[index], transform.TransformPoint(_controller.center), _footStepAudioClipLists.FootstepAudioVolume);
                }
            }
        }
        
        protected virtual void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }
        
        protected virtual void SimulateGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }
            }
            else
            {
                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }
        
        protected virtual float Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = MoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.GetMovement == Vector2.zero)
            {
                targetSpeed = 0.0f;
                _isCharacterMove = false;
            }
            else
            {
                _isCharacterMove = true;
            }

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.GetMovement.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.GetMovement.x, 0.0f, _input.GetMovement.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.GetMovement != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }

            return inputMagnitude;
        }
        
        protected virtual void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(_footStepAudioClipLists.LandingAudioClip, transform.TransformPoint(_controller.center), _footStepAudioClipLists.FootstepAudioVolume);
            }
        }
        
        protected virtual void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }
    }
}