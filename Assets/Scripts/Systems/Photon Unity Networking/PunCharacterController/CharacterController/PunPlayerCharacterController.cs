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
    public class PunPlayerCharacterController : PunCharacterController
    {
        [Header("Skill Path")] 
        [SerializeField] private ResourcesPath _skillConfigPath;
        [SerializeField] private ResourcesPath _skillUpgradePath;
        
        // system
        private WeaponSystem _weaponSystem;
        private PlayerSpawnBullet _playerSpawnBullet;
        
        //Skill
        private SkillPath _skillPath;
        private RandomSkill _randomSkill;
        private ScriptableObject _skill;
        
        public PlayerSpawnBullet playerSpawnBullet
        {
            set => _playerSpawnBullet = value;
        }

        protected override void Awake()
        {
            base.Awake();
            
            _weaponSystem = GetComponent<WeaponSystem>();
            _playerSpawnBullet = GetComponent<PlayerSpawnBullet>();

            _skillPath = new SkillPath();
            _skillPath._skillConfigPath = _skillUpgradePath;
            _skillPath._skillUpgradePath = _skillUpgradePath;
        }


        protected override void OnEnable()
        {
            base.OnEnable();
      
            _randomSkill = GetComponent<RandomSkill>();
            
            if (!photonView.IsMine)
            {
                photonView.RPC("OnGetCharacterData", RpcTarget.MasterClient);
                print($"Awake {gameObject.name}");
            }
        }

        protected override void Start()
        {
            base.Start();
            
            //print($"Up is null : {_skillUpgradePath.paths.Count}");
        }

        protected override void Update()
        {
            base.Update();
        }

        [PunRPC]
        public override void OnGetCharacterData()
        {
            base.OnGetCharacterData();

            if (!photonView.IsMine)
                return;
            
            print($"Get Datas {gameObject.name}");

            object[] datas = new object[5];

            if (_weaponSystem.weaponConfig != null)
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

        [PunRPC]
        public override void RPCInitializeCharacter(object[] datas, int OwnerNetID)
        {
            base.RPCInitializeCharacter(datas, OwnerNetID);

            if(photonView.IsMine)
                return;
            
            print($"Initialize Datas {datas[0]}, {datas[1]}, {datas[2]}");
            _weaponSystem.weaponConfig = new Tuple<WeaponConfig, int>((WeaponConfig)_skillPath.GetSkillFromPath(0, (int)datas[0]),
                (int)datas[0]);

            if ((int)datas[1] != -1)
                _weaponSystem.mainAttachment = new Tuple<WeaponAttachment, int>((WeaponAttachment)_skillPath.GetSkillFromPath(1, (int)datas[1]),
                (int)datas[1]);
            else
                _weaponSystem.mainAttachment = new Tuple<WeaponAttachment, int>(null, 0);
            if((int)datas[2] != -1)
                _weaponSystem.secondaryAttachment = new Tuple<WeaponAttachment, int>((WeaponAttachment)_skillPath.GetSkillFromPath(1, (int)datas[2]),
                (int)datas[2]);
            else
                _weaponSystem.secondaryAttachment = new Tuple<WeaponAttachment, int>(null, 0);
            
            _weaponSystem.weaponConfigStats = JsonConvert.DeserializeObject<WeaponConfigStats>((string)datas[3]);
            _weaponSystem.attachmentStats = JsonConvert.DeserializeObject<WeaponAttachmentStats>((string)datas[4]);
            _weaponSystem.SetWeaponConfig();
            
            if(_weaponSystem.mainAttachment.Item1 != null || _weaponSystem.secondaryAttachment.Item1 != null)
                _weaponSystem.Decorate();
        }

        public void SetSkill(int[] skills, int OwnerNetID)
        {
            print($"ReceiveData [0] : {skills[0]} | [1] : {skills[1]}");
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
            if(photonView.IsMine)
                return;

            switch (skills[1])
            {
                case 0:
                    print($"Path is : {_skillConfigPath.paths[skills[0]]}");
                    print("Index In UI : " + skills[0]);
                    _skill = (WeaponConfig)_skillPath.GetSkillFromPath(0, skills[0]);
                    print($"Skill is null {_skill == null}");
                    photonView.RPC("OnApplyMainSkill", RpcTarget.All, skills[0]);
                    break;
                case 1:
                    print($"Path is : {_skillConfigPath.paths[skills[0]]}");
                    _skill = (WeaponAttachment)_skillPath.GetSkillFromPath(1, skills[0]);
                    print($"Skill is null {_skill == null}");
                    photonView.RPC("OnApplyAttachmentSkill", RpcTarget.All, skills[0]);
                    break;
                case 2:
                    print($"Path is : {_skillUpgradePath.paths[skills[0]]}");
                    _skill = (MainSkillUpgrade)_skillPath.GetSkillFromPath(2, skills[0]);
                    print($"Skill is null {_skill == null}");
                    photonView.RPC("OnUpgradeMainSkill", RpcTarget.All);
                    break;
                case 3:
                    print($"Path is : {_skillUpgradePath.paths[skills[0]]}");
                    _skill = (AttachmentSkillUpgrade)_skillPath.GetSkillFromPath(3, skills[0]);
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

        public override void OnEvent(EventData photonEvent)
        {
            base.OnEvent(photonEvent);
            
            byte eventCode = photonEvent.Code;
            
            if (eventCode == _punEventCode && !photonView.IsMine)
            {
                object[] datas = (object[])photonEvent.CustomData;
                
                //print("This View ID : " + photonView.ViewID + " :: Receive ID : " + (int)data[1]);
                if ((int)datas[0] == photonView.ViewID)
                {
                    //print(data[0] + " : " + gameObject.name);
                    
                    //Invoke Event --------------------
                    //print($"Weapon is null {_weaponSystem == null}");
                    _weaponSystem.ToggleFire(_playerSpawnBullet);
                }
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }
    }
}