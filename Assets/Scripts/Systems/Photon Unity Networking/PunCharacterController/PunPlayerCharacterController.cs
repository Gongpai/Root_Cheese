using System;
using ExitGames.Client.Photon;
using Newtonsoft.Json;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GDD.PUN
{
    public class PunPlayerCharacterController : MonoBehaviourPun, IOnEventCallback
    {
        [Header("Audio")]
        [SerializeField] private CharacterFootStepAudioClipLists _footStepAudioClipLists;

        [Header("Skill Path")] 
        [SerializeField] private string r_skillConfigPath;
        [SerializeField] private string r_skillUpgradePath;
        
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
        private GameManager GM;
        
        //Skill
        private RandomSkill _randomSkill;
        private ResourcesPath _skillConfigPath;
        private ResourcesPath _skillUpgradePath;
        private ScriptableObject _skill;
        
        public PlayerSpawnBullet playerSpawnBullet
        {
            set => _playerSpawnBullet = value;
        }

        public CharacterFootStepAudioClipLists footStepAudioClipLists
        {
            get => _footStepAudioClipLists;
            set => _footStepAudioClipLists = value;
        }

        private void Awake()
        {
            _weaponSystem = GetComponent<WeaponSystem>();
            _playerSpawnBullet = GetComponent<PlayerSpawnBullet>();
        }
        
        

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _randomSkill = GetComponent<RandomSkill>();
            
            if (!photonView.IsMine)
            {
                photonView.RPC("OnGetCharacterData", RpcTarget.MasterClient);
                print($"Awake {gameObject.name}");
            }
        }

        private void Start()
        {
            GM = GameManager.Instance;
            
            AssignAnimationIDs();

            //Get Skill Path
            r_skillConfigPath = _randomSkill.skillConfigPath;
            r_skillUpgradePath = _randomSkill.skillUpgradePath;
            
            //Get ScriptObject
            _skillConfigPath = Resources.Load<ResourcesPath>("Resources_Data/SkillConfigPath");
            _skillUpgradePath = Resources.Load<ResourcesPath>("Resources_Data/SkillUpgradePath");
        }

        private void Update()
        {
            
        }
        
        [PunRPC]
        private void OnGetCharacterData()
        {
            if (photonView.IsMine)
            {
                print($"Get Datas {gameObject.name}");

                object[] datas = new object[5];
                
                if(_weaponSystem.weaponConfig != null)
                    datas[0] = _weaponSystem.weaponConfig.Item2;
                
                if (_weaponSystem.mainAttachment.Item1 != null)
                    datas[1] = _weaponSystem.mainAttachment.Item2;
                else
                    datas[1] = -1;
                
                if (_weaponSystem.secondaryAttachment.Item1 != null)
                    datas[2] = _weaponSystem.secondaryAttachment.Item2;
                else
                    datas[2] = -1;
                
                datas[3] = JsonConvert.SerializeObject(_weaponSystem.weaponConfigStats);
                datas[4] = JsonConvert.SerializeObject(_weaponSystem.attachmentStats);
                
                photonView.RPC("RPCInitializeCharacter", RpcTarget.Others, datas, photonView.ViewID);
            }
        }
        
        [PunRPC]
        public void RPCInitializeCharacter(object[] datas, int OwnerNetID)
        {
            if(photonView.IsMine)
                return;

            print($"Initialize Datas {datas[0]}, {datas[1]}, {datas[2]}");
            _weaponSystem.weaponConfig = new Tuple<WeaponConfig, int>(
                Resources.Load<WeaponConfig>(_skillConfigPath.paths[(int)datas[0]]),
                (int)datas[0]);

            if ((int)datas[1] != -1)
                _weaponSystem.mainAttachment = new Tuple<WeaponAttachment, int>(
                    Resources.Load<WeaponAttachment>(_skillConfigPath.paths[(int)datas[1]]),
                    (int)datas[1]);
            else
                _weaponSystem.mainAttachment = new Tuple<WeaponAttachment, int>(null, 0);
            if((int)datas[2] != -1)
                _weaponSystem.secondaryAttachment = new Tuple<WeaponAttachment, int>(
                    Resources.Load<WeaponAttachment>(_skillConfigPath.paths[(int)datas[2]]),
                    (int)datas[2]);
            else
                _weaponSystem.secondaryAttachment = new Tuple<WeaponAttachment, int>(null, 0);
            
            _weaponSystem.weaponConfigStats = JsonConvert.DeserializeObject<WeaponConfigStats>((string)datas[3]);
            _weaponSystem.attachmentStats = JsonConvert.DeserializeObject<WeaponAttachmentStats>((string)datas[4]);
            _weaponSystem.SetWeaponConfig();
            
            if(_weaponSystem.mainAttachment.Item1 != null || _weaponSystem.secondaryAttachment.Item1 != null)
                _weaponSystem.Decorate();
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
                    //print($"FootstepAudioVolume is null : {_footStepAudioClipLists.FootstepAudioVolume == null}");
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
                //print($"Anim Moveeeeeeeee {gameObject.name}");
                //print($"Speed Data 1.AnimationBlend {_animationBlend} 2.InputMagnitude {inputMagnitude}");
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        public void SetSkill(int[] skills, int OwnerNetID)
        {
            if (photonView != null && photonView.IsMine)
            {
                photonView.RPC("RPCApplySkill", RpcTarget.Others, skills, OwnerNetID);
                //print($"SendData {OwnerNetID}");
            }
            else 
                print("photonView is NULL.");
        }

        [PunRPC]
        public void RPCApplySkill(int[] skills, int OwnerNetID)
        {
            print($"ReceiveData {OwnerNetID}");
            if(photonView.IsMine)
                return;

            switch (skills[1])
            {
                case 0:
                    print($"Path is : {_skillConfigPath.paths[skills[0]]}");
                    print("Index In UI : " + skills[0]);
                    _skill = Resources.Load<WeaponConfig>(_skillConfigPath.paths[skills[0]]);
                    print($"Skill is null {_skill == null}");
                    photonView.RPC("OnApplyMainSkill", RpcTarget.All, skills[0]);
                    break;
                case 1:
                    print($"Path is : {_skillConfigPath.paths[skills[0]]}");
                    _skill = Resources.Load<WeaponAttachment>(_skillConfigPath.paths[skills[0]]);
                    print($"Skill is null {_skill == null}");
                    photonView.RPC("OnApplyAttachmentSkill", RpcTarget.All, skills[0]);
                    break;
                case 2:
                    print($"Path is : {_skillUpgradePath.paths[skills[0]]}");
                    _skill = Resources.Load<MainSkillUpgrade>(_skillUpgradePath.paths[skills[0]]);
                    print($"Skill is null {_skill == null}");
                    photonView.RPC("OnUpgradeMainSkill", RpcTarget.All);
                    break;
                case 3:
                    print($"Path is : {_skillUpgradePath.paths[skills[0]]}");
                    _skill = Resources.Load<AttachmentSkillUpgrade>(_skillUpgradePath.paths[skills[0]]);
                    print($"Skill is null {_skill == null}");
                    photonView.RPC("OnUpgradeAttachmentSkill", RpcTarget.All);
                    break;
            }
        }

        [PunRPC]
        public void OnApplyMainSkill(int index)
        {
            if(photonView.IsMine)
                return;
            
            _weaponSystem.SetMainSkill((WeaponConfig)_skill, index);
        }

        [PunRPC]
        public void OnApplyAttachmentSkill(int index)
        {
            if(photonView.IsMine)
                return;
            
            _weaponSystem.SetAttachment((WeaponAttachment)_skill, index);
        }

        [PunRPC]
        public void OnUpgradeMainSkill()
        {
            if(photonView.IsMine)
                return;
            
            _weaponSystem.UpgradeMainSkill((MainSkillUpgrade)_skill);
        }

        [PunRPC]
        public void OnUpgradeAttachmentSkill()
        {
            if(photonView.IsMine)
                return;
            
            _weaponSystem.UpgradeAttachmentSkill((AttachmentSkillUpgrade)_skill);
        }
        
        public void CallRaiseToggleFireEvent()
        {
            object[] content = new object[]
            {
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
                
                //print("This View ID : " + photonView.ViewID + " :: Receive ID : " + (int)data[1]);
                if ((int)data[0] == photonView.ViewID)
                {
                    //print(data[0] + " : " + gameObject.name);
                    
                    //Invoke Event --------------------
                    //print($"Weapon is null {_weaponSystem == null}");
                    _weaponSystem.ToggleFire(_playerSpawnBullet);
                }
            }
        }
        
        private void OnDisable()
        {
            GM.players.Remove(GetComponent<PlayerSystem>());
            PhotonNetwork.RemoveCallbackTarget(this);
        }
    }
}