using System;
using System.Collections;
using GDD.PUN;
using GDD.Spatial_Partition;
using GDD.Timer;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GDD
{
    public abstract class CharacterSystem : Pawn, ICharacter
    {
        [Header("Player Stats Setting")] 
        [SerializeField] protected TextMeshProUGUI m_hp_text;
        [SerializeField] protected TextMeshProUGUI m_namePlayerText;
        [SerializeField] protected Slider m_hp_bar;
        [SerializeField] protected Slider m_shield_bar;
        [SerializeField] protected float m_hp = 100;
        [SerializeField] protected float m_max_HP = 100;
        [SerializeField] protected float m_shield = 100;
        [SerializeField] protected int _maxEXP = 100;
        [SerializeField] protected float m_levelUp = 1.1f;

        [Header("Animation")] 
        [SerializeField] protected Animator m_animator;
        [SerializeField] protected string m_deadAnimatorState = "isDead";
        [SerializeField] protected string m_reviveAnimatorState = "isRevive";
        [SerializeField] private UnityEvent m_OnDead;
        [SerializeField] private UnityEvent m_OnRevive;
        [SerializeField] private Animator m_HPBarAnimator;
        [SerializeField] private string m_namePlayHPBar = "Play";
        
        [Header("Revive")]
        [SerializeField] private float _reviveTime = 2;
        [SerializeField] protected GameObject m_ReviveUI;
        
        protected bool _isMasterClient = true;
        protected bool _isDead;
        protected Canvas_Element_List _reviveUI;
        protected bool _isOtherPlayerRevive;
        protected int _EXP;
        protected Transform OtherPlayer;
        protected int _currentUpdateEXP;
        protected int _updateEXP;
        protected int _level;
        protected int _skillUpgradeCount;
        protected int _idPhotonView;
        protected AwaitTimer updateEXPTimer;
        protected AwaitTimer timer;
        protected AwaitTimeCounting _reviveCounting;
        protected PunCharacterHealth _punCharacterHealth;

        public bool isMasterClient
        {
            get => _isMasterClient;
            set => _isMasterClient = value;
        }

        public PunCharacterHealth punCharacterHealth
        {
            get
            {
                if (_punCharacterHealth == null)
                    _punCharacterHealth = GetComponent<PunCharacterHealth>();
                
                return _punCharacterHealth;
            }
        }

        public virtual void Awake()
        {
            
        }

        public virtual void OnEnable()
        {
            
        }
        
        public virtual void Start()
        {
            if (isMasterClient)
            {
                _punCharacterHealth = GetComponent<PunCharacterHealth>();
            }

            m_animator = GetComponent<Animator>();
            
            if (m_ReviveUI != null && isMasterClient)
            {
                _reviveUI = Instantiate(m_ReviveUI, Vector3.zero, Quaternion.identity).GetComponent<Canvas_Element_List>();
                _reviveUI.gameObject.SetActive(false);
                _reviveUI.GetComponent<Canvas>().worldCamera = Camera.main;
            }

            updateEXPTimer = new AwaitTimer(1.0f,
                () =>
                {
                    SetUpdateEXP(GetEXP());
                    _currentUpdateEXP = GetEXP();
                },
                time =>
                {
                    SetUpdateEXP((int)Mathf.Lerp(_currentUpdateEXP, GetEXP(), time));
                    //print($"Time : {time} || EXP : {_currentUpdateEXP} || Update : {_updateEXP}");
                });
            timer = new AwaitTimer(5.9f,
                () =>
                {
                    updateEXPTimer.Start();
                },
                time => { });
        }

        public virtual void Update()
        {
            m_hp_bar.value = GetHP() / GetMaxHP();
            
            if(m_hp_text != null)
                m_hp_text.text = $"HP : {GetHP()} / {GetMaxHP()} || Shield : {GetShield()} / {GetMaxShield()}";

            if (m_shield_bar != null && (GetShield() / GetMaxShield()) > 0)
                m_shield_bar.value = GetShield() / GetMaxShield();
            else
                m_shield_bar.value = 0;

            if (GetHP() <= 0 && !_isDead)
            {
                OnCharacterDead();
            }

            LevelProgress();
        }

        protected virtual void LevelProgress()
        {
            if (GetEXP() >= GetMaxEXP())
            {
                SetLevel(GetLevel() + 1);
                SetEXP(GetEXP() - GetMaxEXP());
                SetMaxEXP((int)(GetMaxEXP() * m_levelUp));
                _skillUpgradeCount++;
                
                OnLevelUP();
            }
        }

        protected virtual void OnLevelUP()
        {
            
        }

        protected void OnEXPAdd()
        {
            if (GetUpdateEXP() != GetEXP())
            {
                timer.Start();
            }
            else
            {
                _currentUpdateEXP = GetUpdateEXP();
            }
        }

        public int idPhotonView
        {
            get => _idPhotonView;
            set => _idPhotonView = value;
        }

        protected virtual void OnGUI()
        {
            
        }

        public virtual void OnCharacterDead()
        {
            m_OnDead?.Invoke();
            m_animator.SetTrigger(m_deadAnimatorState);
            _isDead = true;
        }

        public void ReviveButton(bool isRevive)
        {
            if (!_isOtherPlayerRevive || !isMasterClient)
            {
                print($"Cannot Revive Button");
                return;
            }

            if (isRevive)
            {
                _reviveCounting = new AwaitTimeCounting(time =>
                {
                    print("Revive Time : " + time);
                    _reviveUI.images[0].fillAmount = time / _reviveTime;

                    if (time >= _reviveTime)
                    {
                        _reviveCounting.Stop();
                        
                        if(OtherPlayer != null)
                            ReviveResetHP(OtherPlayer.parent.gameObject);
                    }
                }, () =>
                {
                    _reviveUI.images[0].fillAmount = 0;
                });
                _reviveCounting.Start();
            }
            else
            {
                _reviveCounting.Stop();
            }
        }
        
        public virtual void OnReviveTriggerEnter(Collider other)
        {
            if(!isMasterClient)
                return;
            
            if (other.CompareTag("Revive") && !_reviveUI.gameObject.activeSelf)
            {
                print("Revive Enter");
                _isOtherPlayerRevive = other.transform.parent.GetComponent<CharacterSystem>().GetHP() <= 0 && GetHP() > 0;
                _reviveUI.gameObject.SetActive(_isOtherPlayerRevive);
                OtherPlayer = other.transform;
            }
        }
        
        public virtual void OnReviveTriggerExit(Collider other)
        {
            if(!isMasterClient)
                return;
            
            if (other.CompareTag("Revive"))
            {
                print("Revive Exit");
                _reviveUI.gameObject.SetActive(false);
                _isOtherPlayerRevive = false;
                OtherPlayer = null;
            }
        }

        public virtual void PlayHPBarAttack()
        {
            m_HPBarAnimator.SetTrigger(m_namePlayHPBar);
        }

        public virtual void ReviveResetHP(GameObject other)
        {
            if(!isMasterClient)
                return;
            
            other.GetComponent<CharacterSystem>().OnCharacterRevive();
        }

        public void OnCharacterRevive()
        {
            m_animator.SetTrigger(m_reviveAnimatorState);
            SetHP(GetMaxHP());
            m_OnRevive?.Invoke();
            _isDead = false;
        }
        
        public virtual void OnDisable()
        {
            _isDead = true;
        }

        public virtual float GetHP()
        {
            return m_hp;
        }

        public virtual void SetMaxHP(float maxHP)
        {
            m_max_HP = maxHP;
        }

        public virtual float GetMaxHP()
        {
            return m_max_HP;
        }

        public virtual void SetHP(float hp)
        {
            if (hp >= GetMaxHP())
                m_hp = GetMaxHP();
            else
                m_hp = hp;
        }

        public virtual float GetMaxShield()
        {
            return 100;
        }

        public virtual float GetShield()
        {
            return m_shield;
        }

        public virtual void SetShield(float shield)
        {
            if (shield >= GetMaxShield())
                m_shield = GetMaxShield();
            else
                m_shield = shield;
        }

        public virtual void SetMaxEXP(int maxEXP)
        {
            _maxEXP = maxEXP;
        }

        public virtual int GetMaxEXP()
        {
            return _maxEXP;
        }

        public virtual void AddEXP(int EXP)
        {
            SetEXP(GetEXP() + EXP);
            OnEXPAdd();
        }
        
        public virtual void SetUpdateEXP(int EXP)
        {
            _updateEXP = EXP;
        }

        public virtual void SetEXP(int EXP)
        {
            _EXP = EXP;
        }

        public virtual int GetEXP()
        {
            return _EXP;
        }

        public virtual int GetUpdateEXP()
        {
            return _updateEXP;
        }

        public virtual int GetLevel()
        {
            return _level;
        }

        public virtual void SetLevel(int level)
        {
            _level = level;
        }

        public virtual void SetMaxWalkSpeed(float speed)
        {
            
        }

        public override Transform GetPawnTransform()
        {
            if (this == null)
                return null;
            
            return transform;
        }

        public virtual void OnDestroy()
        {
            
        }
    }
}