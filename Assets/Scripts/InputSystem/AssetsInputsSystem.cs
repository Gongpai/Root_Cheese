using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace GDD
{
    public class AssetsInputsSystem : MonoBehaviour
    {
        [Header("Movement Settings")]
        public bool analogMovement;
        
        [Header("UI For Debug Input Values")] [SerializeField]
        private GameObject m_debug_ui;
        
        //Input Vector2 Movement
        private Vector2 _movement = new Vector2();
        
        //Ready Button
        private UnityAction readyAction;
        
        //Revive Button
        private UnityAction<bool> reviveAction;
        
        public Vector2 GetMovement
        {
            get => _movement;
        }

        public UnityAction Ready
        {
            get => readyAction;
            set => readyAction = value;
        }
        
        public UnityAction<bool> Revive
        {
            get => reviveAction;
            set => reviveAction = value;
        }

        //Get Input Value From Input System
        public void OnMovement(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
            //print("Input H : " + _movement.x + " | Input V : " + _movement.y);
        }

        public void OnReady(InputValue value)
        {
            readyAction?.Invoke();
        }
        
        //Set Input Value To Var
        public void MoveInput(Vector2 newMoveDirection)
        {
            _movement = newMoveDirection;
        }

        public void OnRevive(InputValue value)
        {
            reviveAction?.Invoke(value.isPressed);
        }

        private void Update()
        {
            //print("Input H : " + _movement.horizontal + " | Input V : " + _movement.vertical);
        }
    }
}

//Code By Kongphai Wutthichaiya - Github Gongpai