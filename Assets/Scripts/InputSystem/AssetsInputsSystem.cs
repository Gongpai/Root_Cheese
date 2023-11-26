using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        public Vector2 GetMovement
        {
            get => _movement;
        }

        //Get Input Value From Input System
        public void OnMovement(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
            //print("Input H : " + _movement.x + " | Input V : " + _movement.y);
        }
        
        //Set Input Value To Var
        public void MoveInput(Vector2 newMoveDirection)
        {
            _movement = newMoveDirection;
        }

        private void Update()
        {
            //print("Input H : " + _movement.horizontal + " | Input V : " + _movement.vertical);
        }
    }
}

//Code By Kongphai Wutthichaiya - Github Gongpai