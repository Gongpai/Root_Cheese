using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace GDD.PUN
{
    public class PunPlayerCharacterController : MonoBehaviourPun, IOnEventCallback
    {
        [Header("Audio")]
        [SerializeField] private CharacterFootStepAudioClipLists _footStepAudioClipLists;
        
        //Setting
        private float _moveSpeed = 2.0f;
        private float SpeedChangeRate = 10.0f;
        
        //player
        private float _speed;
        private float _animationBlend;
        private float inputMagnitude;
        
        // animation IDs
        private int _animIDSpeed;
        private int _animIDMotionSpeed;
        
        // system
        private Animator _animator;
        private bool _hasAnimator;
        private byte _punEventCode = 10;
        private CharacterController _controller;
        private WeaponSystem _weaponSystem;
        private PlayerSpawnBullet _playerSpawnBullet;

        public WeaponSystem weaponSystem
        {
            set => _weaponSystem = value;
        }

        public PlayerSpawnBullet playerSpawnBullet
        {
            set => _playerSpawnBullet = value;
        }

        public CharacterFootStepAudioClipLists footStepAudioClipLists
        {
            get => _footStepAudioClipLists;
            set => _footStepAudioClipLists = value;
        }
        
        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
        }

        private void Start()
        {
            AssignAnimationIDs();
        }

        private void Update()
        {
            
        }
        
        private void AssignAnimationIDs()
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
                    print($"FootstepAudioVolume is null : {_footStepAudioClipLists.FootstepAudioVolume == null}");
                    AudioSource.PlayClipAtPoint(_footStepAudioClipLists.FootstepAudioClips[index], transform.TransformPoint(_controller.center), _footStepAudioClipLists.FootstepAudioVolume);
                }
            }
        }

        public void SetSpeed(float[] amount, int OwnerNetID)
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
        public void RPCApplyCharacterSpeed(float[] amount, int OwnerNetID)
        {
            _animationBlend = amount[0];
            inputMagnitude = amount[1];
            //print($"ReceiveData {OwnerNetID}");
            photonView.RPC("OnMove", RpcTarget.Others);
        }

        [PunRPC]
        private void OnMove()
        {
            if (_hasAnimator)
            {
                print($"Anim Moveeeeeeeee {gameObject.name}");
                print($"Speed Data 1.AnimationBlend {_animationBlend} 2.InputMagnitude {inputMagnitude}");
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }
        
        public void CallRaiseToggleFireEvent()
        {
            object[] content = new object[]
            {
                "Event!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!",
                photonView.ViewID
            };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions()
            {
                Receivers = ReceiverGroup.All
            };
            SendOptions sendOptions = new SendOptions()
            {
                Reliability = true,
                Encrypt = true
            };

            PhotonNetwork.RaiseEvent(_punEventCode, content, raiseEventOptions, sendOptions);
        }
        
        public void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;
            
            if (eventCode == _punEventCode && !photonView.IsMine)
            {
                object[] data = (object[])photonEvent.CustomData;
                
                print("This View ID : " + photonView.ViewID + " :: Receive ID : " + (int)data[1]);
                if ((int)data[1] == photonView.ViewID)
                {
                    print(data[0] + " : " + gameObject.name);
                    
                    //Invoke Event --------------------
                    _weaponSystem.ToggleFire(_playerSpawnBullet);
                }
            }
        }
        
        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }
    }
}