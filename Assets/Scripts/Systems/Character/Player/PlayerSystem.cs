using System;
using System.Collections.Generic;
using GDD.PUN;
using GDD.Spatial_Partition;
using GDD.StateMachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace GDD
{
    public class PlayerSystem : CharacterSystem
    {
        [Header("Random UI")] 
        [SerializeField] private GameObject m_skillRandomUI;
        [SerializeField] private GameObject m_randomSpinWheel;
        
        [Header("UI")]
        [SerializeField] private ReadyCheckUI _readyCheck;
        
        [Header("Player Attack Setting")]
        [SerializeField][Tooltip("Time For Delay Enter Attack State")] 
        private float m_delay_attack = 0.5f;

        [SerializeField] [Tooltip("Player vision to find enemy")]
        private Vector2 vision;
        
        private CharacterControllerSystem _controllerSystem;
        private IState<PlayerSystem> _attackState, _moveState;
        private StateContext<PlayerSystem> _playerStateContext;
        private WeaponSystem _weaponSystem;
        private RandomSkill _randomSkill;
        private RandomSkillUI _randomSkillUI;
        private GameManager GM;
        private PunCharacterHealth _punCharacterHealth;
        private bool _isEnterRoom;
        List<UnityAction<float, float>> _spinWheelActions = new List<UnityAction<float, float>>();
        
        public float delay_attack
        {
            get => m_delay_attack;
        }

        public Vector2 Get_Vision
        {
            get => vision;
        }
        
        public float shield
        {
            get => m_shield;
            set => m_shield = value;
        }

        public override void OnEnable()
        {
            base.OnEnable();
        }

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();

            _weaponSystem = GetComponent<WeaponSystem>();

            if (isMasterClient)
            {
                _controllerSystem = GetComponent<CharacterControllerSystem>();
                _controllerSystem.input.Ready = ReadyButton;
                _punCharacterHealth = GetComponent<PunCharacterHealth>();

                SetSpinWheelAction();
            }
            
            _readyCheck.gameObject.SetActive(false);
            _playerStateContext = new StateContext<PlayerSystem>(this);
            
            //SinglePlayer AttackState
            //_attackState = gameObject.AddComponent<PlayerAttackState>();
            
            //Multiplayer AttackState
            _attackState = gameObject.AddComponent<MultiplayerAttackState>();
            
            //MoveState
            _moveState = gameObject.AddComponent<PlayerMoveState>();
            
            _randomSkill = GetComponent<RandomSkill>();
            _randomSkill.weaponSystem = _weaponSystem;
            _randomSkill.OnInitialize();

            GM = GameManager.Instance;
            
            //Add Player to GameManager
            GM.players.Add(this, false);

            /*
            GameObject r_skill_ui = Instantiate(m_skillRandomUI);
            _randomSkillUI = r_skill_ui.transform.GetChild(0).GetComponent<RandomSkill_UI>();
            _randomSkillUI.randomSkill = _randomSkill;
            _randomSkillUI.OnCreate();
            */
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
            
            if(!_isMasterClient)
                return;
            
            /*
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject r_skill_ui = Instantiate(m_skillRandomUI);
                _randomSkillUI = r_skill_ui.transform.GetChild(0).GetComponent<RandomSkillUI>();
                _randomSkillUI.randomSkill = _randomSkill;
                _randomSkillUI.OnCreate();
            }*/
            
            if(_controllerSystem.Get_Player_Move)
                StartMove();
            else
                StartAttack();
        }

        private void ReadyButton()
        {
            if(GM.enemies.Count > 0)
                return;
            
            if (_isEnterRoom)
            {
                GM.players[this] = !GM.players[this];
                _readyCheck.ready = GM.players[this];
                
                GM.OnReady();
                PunRoomManager.Instance.CreateUpdateReadyNextLevelPlayer();
            }
        }

        public void UpdateReadyCheckUI()
        {
            _readyCheck.ready = GM.players[this];
            GM.OnReady();
        }

        private void SetSpinWheelAction()
        {
            //Reward
            for (int i = 0; i < 5; i++)
            {
                _spinWheelActions.Add((amountA, amountB) =>
                {
                    SetHPFromSpinWheel(amountA);
                });
            }
            for (int i = 0; i < 3; i++)
            {
                _spinWheelActions.Add((amountA, amountB) =>
                {
                    SetShieldFromSpinWheel(amountA);
                    SetHPFromSpinWheel(amountB);
                });
            }
            for (int i = 0; i < 2; i++)
            {
                _spinWheelActions.Add((amountA, amountB) =>
                {
                    SetEXPFromSpinWheel(amountA);
                });
            }
        }
        
        public void OnDoorEnter(Collider other)
        {
            if(GM != null && GM.enemies.Count > 0)
                return;
            
            if (other.tag == "Door")
            {
                _readyCheck.gameObject.SetActive(true);
                _isEnterRoom = true;
                _readyCheck.ready = GM.players[this];
            }
        }

        public void OnDoorExit(Collider other)
        {
            if (other.tag == "Door")
            {
                _isEnterRoom = false;
                GM.players[this] = false;
                _readyCheck.gameObject.SetActive(false);
                GM.OnReady();
            }
        }

        public void OnEnterSpinWheel(Collider other)
        {
            if(!isMasterClient)
                return;
            
            if (other.tag == "SpinWheel")
            {
                OpenRandomSpinWheelUI();
                other.GetComponent<IInteract>().OnInteract();
            }
        }

        protected override void OnLevelUP()
        {
            base.OnLevelUP();
            
            OpenRandomSkillUI();
        }

        public void OpenRandomSkillUI()
        {
            if(!isMasterClient)
                return;
            
            if (_skillUpgradeCount > 0 && GM.enemies.Count <= 0)
            {
                print("OnRandomSkillPlayers");
                _skillUpgradeCount--;
                
                GameObject r_skill_ui = Instantiate(m_skillRandomUI);
                _randomSkillUI = r_skill_ui.transform.GetChild(0).GetComponent<RandomSkillUI>();
                _randomSkillUI.randomSkill = _randomSkill;

                if (_skillUpgradeCount > 1)
                    _randomSkillUI.reOpenUIAction = OpenRandomSkillUI;
                
                _randomSkillUI.OnCreate();
            }
        }

        public void OpenRandomSpinWheelUI()
        {
            if(!isMasterClient)
                return;
            
            GameObject r_spinWheel = Instantiate(m_randomSpinWheel);
            RandomCharacterStatusSpinWheelUI r_spinWheelUI = r_spinWheel.GetComponent<Canvas_Element_List>().canvas_gameObjects[0].GetComponent<RandomCharacterStatusSpinWheelUI>();
            
            r_spinWheelUI.resultActions = _spinWheelActions;
        }

        private void SetHPFromSpinWheel(float amount)
        {
            print($"Set HP Amount = {GetHP() + GetMaxHP() * (amount / 100)}");
            float hpAmount = GetMaxHP() * (amount / 100);
            SetHP(GetHP() + hpAmount); 
            _punCharacterHealth.HealingPoint(hpAmount, _idPhotonView);
        }
        
        private void SetEXPFromSpinWheel(float amount)
        {
            print($"Set EXP Amount = {GetMaxEXP() * (amount / 100.0f)}");
            AddEXP((int)(GetMaxEXP() * (amount / 100.0f)));
        }
        
        private void SetShieldFromSpinWheel(float amount)
        { 
            print($"Set Shield Amount = {GetShield() + GetMaxShield() * (amount / 100)}");
            float shieldAmount = GetMaxShield() * (amount / 100);
            SetShield(GetShield() + shieldAmount);
            _punCharacterHealth.ShieldPoint(shieldAmount, _idPhotonView);
        }
        
        public override float GetMaxShield()
        {
            if(_weaponSystem.mainAttachment.Item1 != null && _weaponSystem.mainAttachment.Item1.shield != 0)
                return _weaponSystem.attachmentStats.shield * 100 + _weaponSystem.mainAttachment.Item1.shield;
            if(_weaponSystem.secondaryAttachment.Item1 != null && _weaponSystem.secondaryAttachment.Item1.shield != 0)
                return _weaponSystem.attachmentStats.shield * 100 + _weaponSystem.secondaryAttachment.Item1.shield;
            
            if (_weaponSystem == null || _weaponSystem.mainAttachment.Item1 == null || _weaponSystem.secondaryAttachment.Item1 == null)
                return 0.0f;

            return 0.0f;
        }

        public override float GetShield()
        {
            if(_weaponSystem != null)
                if (_weaponSystem.mainAttachment.Item1 != null || _weaponSystem.secondaryAttachment.Item1 != null)
                    if(_weaponSystem.mainAttachment.Item1.shield > 0 || _weaponSystem.secondaryAttachment.Item1.shield > 0)
                        return base.GetShield();

            return 0.0f;
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            
            if(GUI.Button(new Rect(20,20,150, 50), "Add EXP"))
                AddEXP(25);
            
            if(_isMasterClient)
                return;
            
            GUI.contentColor = Color.magenta;
            GUI.Label(new Rect(10, 10, 600, 20), "CONFIG -------------------------");

            if (_weaponSystem.weaponConfig != null)
            {
                GUI.Label(new Rect(10, 40, 600, 20), "Main Skill = " + _weaponSystem.weapon.mainName);
                GUI.Label(new Rect(250, 40, 600, 20), "Damage = " + _weaponSystem.weapon.damage);
                GUI.Label(new Rect(350, 40, 600, 20), "Power = " + _weaponSystem.weapon.power);
                GUI.Label(new Rect(450, 40, 600, 20), "Power = " + _weaponSystem.weapon.rate);
            }
            else
                GUI.Label(new Rect(10, 40, 600, 20), "Main Skill = -null-");

            if (_weaponSystem.mainAttachment.Item1 != null)
                GUI.Label(new Rect(10, 70, 600, 20), "Main Attachment Skill = " + _weaponSystem.weapon.mainAttachmentName);
            else
                GUI.Label(new Rect(10, 70, 600, 20), "Main Attachment Skill = -null-");
            
            if(_weaponSystem.secondaryAttachment.Item1 != null)
                GUI.Label(new Rect(10, 100, 600, 20), "Secondary Attachment Skill = " + _weaponSystem.weapon.secAttachmentName);
            else
                GUI.Label(new Rect(10, 100, 600, 20), "Secondary Attachment Skill = -null-");
            
            GUI.contentColor = Color.blue;
            GUI.Label(new Rect(10, 130, 600, 20), "Upgrade Attachment -------------------------");
            GUI.Label(new Rect(10, 160, 600, 20), "MaxHealth = " + GetMaxHP());
            GUI.Label(new Rect(10, 190, 600, 20), "Sheild = " + _weaponSystem.attachmentStats.shield);
            GUI.Label(new Rect(10, 220, 600, 20), "Effect Health = " + _weaponSystem.attachmentStats.effect_health);
            GUI.Label(new Rect(10, 250, 600, 20), "ATT Spin Speed = " + _weaponSystem.attachmentStats.attachmentSpinSpeed);
            GUI.Label(new Rect(10, 280, 600, 20), "ATT Damage = " + _weaponSystem.attachmentStats.attachmentDamage);
            
            GUI.contentColor = Color.red;
            GUI.Label(new Rect(10, 310, 600, 20), "Upgrade Main -------------------------");
            GUI.Label(new Rect(10, 340, 600, 20), "Main Damage = " + _weaponSystem.weaponConfigStats.damage);
            GUI.Label(new Rect(10, 370, 600, 20), "Main Power = " + _weaponSystem.weaponConfigStats.power);
            GUI.Label(new Rect(10, 400, 600, 20), "Main Rate = " + _weaponSystem.weaponConfigStats.rate);
        }

        public void StartAttack()
        {
            _playerStateContext.Transition(_attackState);
        }

        public void StartMove()
        {
            _playerStateContext.Transition(_moveState);
        }

        //Spatial Partition Enemy Finding
        public override Vector2 GetPawnVision()
        {
            return vision;
        }
        public override void SetPawnVision(Vector2 vision)
        {
            this.vision = vision;
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            
            if(GM != null)
                GM.players.Remove(this);
        }
    }
}