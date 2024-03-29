﻿using System;
using GDD.PUN;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GDD
{
    public class RandomSkillUI : MonoBehaviour
    {
        [Header("Add Random Skill To Viewport")]
        [SerializeField] private GameObject m_viewport;
        
        [Header("Prefab Create Skill Button")]
        [SerializeField] private GameObject m_skill_Gruop;
        [SerializeField] private GameObject m_skill_Element;

        [Header("Prefab Random")] [SerializeField]
        private GameObject m_preRandom;
        
        [Header("Add Description Skill To UI")]
        [SerializeField] private GameObject m_description_group;
        [Header("Prefab Create Description Skill")]
        [SerializeField] private GameObject m_description_Element;

        [Header("Random Skill Script")]
        [SerializeField] private RandomSkill _randomSkill;
        [SerializeField] private ScrollViewForAnimation _scrollViewAnim;
        [SerializeField] protected Animator m_animator;

        [Header("Level")] 
        [SerializeField] private TextMeshProUGUI m_levelText;
        [SerializeField] private string m_first = "You have reached level : ";
        [SerializeField] private string m_end;
        
        protected AnimationStateAction _animationStateAction;

        //Pun System
        private PunPlayerCharacterController _punPlayerController;
        
        //Event
        private UnityAction _reOpenUIAction;

        public RandomSkill randomSkill
        {
            set => _randomSkill = value;
        }

        public UnityAction reOpenUIAction
        {
            get => _reOpenUIAction;
            set => _reOpenUIAction = value;
        }

        private void Awake()
        {
            if (m_animator == null)
                m_animator = GetComponent<Animator>();
            //_animationStateAction = m_animator.GetBehaviour<AnimationStateAction>();
            //_animationStateAction.OnStateUpdateAction += OnEndAction;
        }

        private void Update()
        {
            if(GameStateManager.Instance.currentGameState == GameState.Playing)
                GameStateManager.Instance.SetState(GameState.Pause);
        }

        protected virtual void OnEndAction(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).length <=
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                Destroy(gameObject);
            }
        }

        public void OnCreate(int level)
        {
            m_levelText.text = m_first + level + m_end;
            
            _scrollViewAnim.enabled = false;

            GameObject group = Instantiate(m_skill_Gruop);
            group.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            _randomSkill.OnRandomSkill(RandomSkillType.All, 3);
            _punPlayerController = _randomSkill.GetComponent<PunPlayerCharacterController>();
            
            //BaseSkill
            foreach (var baseSkill in _randomSkill.baseSkills)
            {
                //Random Skill
                GameObject element = Instantiate(m_skill_Element);
                Transform child_element = element.transform.GetChild(0);
                Image element_image = child_element.GetComponent<Image>();
                Button skill_button = child_element.GetComponent<Button>();

                //Description Skill
                GameObject d_element = Instantiate(m_description_Element);
                TextMeshProUGUI _skillName = d_element.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI _skillDescription = d_element.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

                if (baseSkill.Item1 != null)
                {
                    element_image.sprite = baseSkill.Item1.skillIcon;
                    _skillName.text = baseSkill.Item1.weaponName;
                    _skillDescription.text = baseSkill.Item1.skillDescription;
                    skill_button.onClick.AddListener((() =>
                    {
                        print("Index Begin In UI : " + baseSkill.Item3);
                        //Set Host
                        _randomSkill.weaponSystem.SetMainSkill(baseSkill.Item1, baseSkill.Item3);

                        //Set Client
                        int[] skills = new int[2] { baseSkill.Item3, 0};
                        print("Index In UI : " + skills[0]);
                        int OwnerNetID = _punPlayerController.photonView.ViewID;
                        _punPlayerController.SetSkill(skills, OwnerNetID);
                        
                        //ReOpenUI
                        _reOpenUIAction?.Invoke();
                        
                        Destroy(transform.parent.gameObject);
                    }));
                }
                else
                {
                    element_image.sprite = baseSkill.Item2.skillIcon;
                    _skillName.text = baseSkill.Item2.attachmentName;
                    _skillDescription.text = baseSkill.Item2.skillDescription;
                    skill_button.onClick.AddListener((() =>
                    {
                        //Set Host
                        _randomSkill.weaponSystem.SetAttachment(baseSkill.Item2, baseSkill.Item3);
                        
                        //Set Client
                        int[] skills = new int[2] { baseSkill.Item3, 1};
                        int OwnerNetID = _punPlayerController.photonView.ViewID;
                        _punPlayerController.SetSkill(skills, OwnerNetID);
                        
                        //ReOpenUI
                        _reOpenUIAction?.Invoke();
                        
                        Destroy(transform.parent.gameObject);
                    }));
                }

                //Add to Group
                element.transform.parent = group.transform;
                d_element.transform.parent = m_description_group.transform;

            }
            
            //UpgradeSkill
            foreach (var upgradeSkill in _randomSkill.upgradeSkills)
            {
                //Random Skill
                GameObject element = Instantiate(m_skill_Element);
                Transform child_element = element.transform.GetChild(0);
                Image element_image = child_element.GetComponent<Image>();
                Button skill_button = child_element.GetComponent<Button>();

                //Description Skill
                GameObject d_element = Instantiate(m_description_Element);
                TextMeshProUGUI _skillName = d_element.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI _skillDescription = d_element.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

                if (upgradeSkill.Item1 != null)
                {
                    element_image.sprite = upgradeSkill.Item1.skillIcon;
                    _skillName.text = upgradeSkill.Item1.skillName;
                    _skillDescription.text = upgradeSkill.Item1.skillDescription;
                    skill_button.onClick.AddListener((() =>
                    {
                        //Set Host
                        _randomSkill.weaponSystem.UpgradeMainSkill(upgradeSkill.Item1);
                        
                        //Set Client
                        int[] skills = new int[2] { upgradeSkill.Item3, 2};
                        print("Index In UI : " + skills[0]);
                        int OwnerNetID = _punPlayerController.photonView.ViewID;
                        _punPlayerController.SetSkill(skills, OwnerNetID);
                        
                        //ReOpenUI
                        _reOpenUIAction?.Invoke();
                        
                        Destroy(transform.parent.gameObject);
                    }));
                }
                else
                {
                    element_image.sprite = upgradeSkill.Item2.skillIcon;
                    _skillName.text = upgradeSkill.Item2.skillName;
                    _skillDescription.text = upgradeSkill.Item2.skillDescription;
                    skill_button.onClick.AddListener((() =>
                    {
                        //Set Host
                        _randomSkill.weaponSystem.UpgradeAttachmentSkill(upgradeSkill.Item2);
                        
                        //Set Client
                        int[] skills = new int[2] { upgradeSkill.Item3, 3};
                        int OwnerNetID = _punPlayerController.photonView.ViewID;
                        _punPlayerController.SetSkill(skills, OwnerNetID);
                        
                        //ReOpenUI
                        _reOpenUIAction?.Invoke();
                        
                        Destroy(transform.parent.gameObject);
                    }));
                }

                //Add to Group
                element.transform.parent = group.transform;
                d_element.transform.parent = m_description_group.transform;
            }

            GameObject preRandom = Instantiate(m_preRandom);
            preRandom.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            group.transform.parent = preRandom.transform;
            preRandom.transform.parent = m_viewport.transform;

            _scrollViewAnim.content = preRandom.transform.GetComponent<RectTransform>();
            _scrollViewAnim.enabled = true;
        }
        
        private void OnDestroy()
        {
            GameStateManager.Instance.SetState(GameState.Playing);
            //_animationStateAction.OnStateUpdateAction -= OnEndAction;
        }
    }
}