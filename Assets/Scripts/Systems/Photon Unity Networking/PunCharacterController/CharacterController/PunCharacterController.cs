using System;
using ExitGames.Client.Photon;
using Newtonsoft.Json;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GDD.PUN
{
    public class PunCharacterController : MonoBehaviourPun, IOnEventCallback
    {
        [Header("Audio")]
        [SerializeField] private CharacterFootStepAudioClipLists _footStepAudioClipLists;
        
        //Setting
        protected float _moveSpeed = 2.0f;
        protected float SpeedChangeRate = 10.0f;
        
        //player
        protected float _speed;
        protected float _animationBlend;
        protected float inputMagnitude;
        
        // animation IDs
        protected int _animIDSpeed;
        protected int _animIDMotionSpeed;
        
        // system
        protected Animator _animator;
        protected bool _hasAnimator;
        protected byte _punEventCode = 10;
        protected CharacterController _controller;
        protected GameManager GM;

        public CharacterFootStepAudioClipLists footStepAudioClipLists
        {
            get => _footStepAudioClipLists;
            set => _footStepAudioClipLists = value;
        }

        protected virtual void Awake()
        {
            
        }
        
        protected virtual void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
        }

        protected virtual void Start()
        {
            GM = GameManager.Instance;
            
            AssignAnimationIDs();
        }

        protected virtual void Update()
        {
            
        }
        
        [PunRPC]
        public virtual void OnGetCharacterData()
        {
            
        }
        
        [PunRPC]
        public virtual void RPCInitializeCharacter(object[] datas, int OwnerNetID)
        {
            
        }
        
        protected virtual void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }
        
        protected virtual void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f && !photonView.IsMine)
            {
                if (_footStepAudioClipLists.FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, _footStepAudioClipLists.FootstepAudioClips.Length);
                    //print($"FootstepAudioVolume is null : {_footStepAudioClipLists.FootstepAudioVolume == null}");
                    AudioSource.PlayClipAtPoint(_footStepAudioClipLists.FootstepAudioClips[index], transform.TransformPoint(_controller.center), _footStepAudioClipLists.FootstepAudioVolume);
                }
            }
        }

        public virtual void SetSpeed(float[] amount, int OwnerNetID)
        {
            if (photonView != null && photonView.IsMine)
            {
                photonView.RPC("RPCApplyCharacterSpeed", RpcTarget.All, amount, OwnerNetID);
                //print($"SendData {OwnerNetID}");
            }
            else 
                print("photonView is NULL.");
        }
        
        [PunRPC]
        public virtual void RPCApplyCharacterSpeed(float[] amount, int OwnerNetID)
        {
            _animationBlend = amount[0];
            inputMagnitude = amount[1];
            //print($"ReceiveData {OwnerNetID}");
            photonView.RPC("OnMove", RpcTarget.Others);
        }

        [PunRPC]
        protected virtual void OnMove()
        {
            if (_hasAnimator)
            {
                //print($"Anim Moveeeeeeeee {gameObject.name}");
                //print($"Speed Data 1.AnimationBlend {_animationBlend} 2.InputMagnitude {inputMagnitude}");
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }
        
        public virtual void OnEvent(EventData photonEvent)
        {
            
        }
        
        protected virtual void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }
    }
}