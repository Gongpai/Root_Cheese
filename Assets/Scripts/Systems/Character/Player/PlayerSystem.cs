using System;
using GDD.Spatial_Partition;
using GDD.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GDD
{
    public class PlayerSystem : CharacterSystem
    {
        [Header("Level")] 
        [SerializeField] private GameObject m_skillRandomUI;
        [SerializeField] private float m_maxLevel;
        
        [Header("Player Attack Setting")]
        [SerializeField][Tooltip("Time For Delay Enter Attack State")] 
        private float m_delay_attack = 0.5f;

        [SerializeField] [Tooltip("Player vision to find enemy")]
        private Vector2 vision;
        
        private PlayerCharacterController _playerController;
        private IState<PlayerSystem> _attackState, _moveState;
        private StateContext<PlayerSystem> _playerStateContext;
        private WeaponSystem _weaponSystem;
        private RandomSkill _randomSkill;
        private RandomSkill_UI _randomSkillUI;
        private float _level;
        private GameManager GM;

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
        
        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();

            _weaponSystem = GetComponent<WeaponSystem>();
            _playerController = GetComponent<PlayerCharacterController>();
            _playerStateContext = new StateContext<PlayerSystem>(this);
            _attackState = gameObject.AddComponent<PlayerAttackState>();
            _moveState = gameObject.AddComponent<PlayerMoveState>();
            
            _randomSkill = GetComponent<RandomSkill>();
            _randomSkill.weaponSystem = _weaponSystem;
            _randomSkill.OnInitialize();

            GM = GameManager.Instance;
            GM.players.Add(this);

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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject r_skill_ui = Instantiate(m_skillRandomUI);
                _randomSkillUI = r_skill_ui.transform.GetChild(0).GetComponent<RandomSkill_UI>();
                _randomSkillUI.randomSkill = _randomSkill;
                _randomSkillUI.OnCreate();
            }
            
            if(_playerController.Get_Player_Move)
                StartMove();
            else
                StartAttack();
        }

        private void OnGUI()
        {
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

            if (_weaponSystem.mainAttachment != null)
                GUI.Label(new Rect(10, 70, 600, 20), "Main Attachment Skill = " + _weaponSystem.weapon.mainAttachmentName);
            else
                GUI.Label(new Rect(10, 70, 600, 20), "Main Attachment Skill = -null-");
            
            if(_weaponSystem.secondaryAttachment != null)
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
    }
}