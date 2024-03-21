using System;
using GDD.PUN;
using GDD.Timer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GDD
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PunPlayerCharacterController))]
    public class MultiplayerPlayerControllerSystem : CharacterControllerSystem<PlayerSystem>
    {
        private PunPlayerCharacterController _punPlayerController;
        private bool _haspunPlayerController;
        private bool _isMine;
        private GameManager GM;

        public bool isMine
        {
            set => _isMine = value;
        }
        
        protected override void Start()
        {
            base.Start();
            
            Ready();
            Revive();
            _haspunPlayerController = TryGetComponent(out _punPlayerController);
            _punPlayerController = GetComponent<PunPlayerCharacterController>();
            
            if(_haspunPlayerController)
                _punPlayerController.footStepAudioClipLists = _footStepAudioClipLists;
        }

        protected override void Update()
        {
            base.Update();
            
            DetectedPlayerMove();
        }

        protected override void OnGameStateChanged(GameState gameState)
        {
            if(!_isMine)
                return;
            
            base.OnGameStateChanged(gameState);
        }

        protected void Ready()
        {
            if(!_characterSystem.isMasterClient)
                return;
            
            _input.Ready = () =>
            {
                _characterSystem.ReadyButton();
            };
        }

        protected void Revive()
        {
            if(!_characterSystem.isMasterClient)
                return;
            
            _input.Revive = isPressed =>
            {
                _characterSystem.ReviveButton(isPressed);
            };
        }

        protected void DetectedPlayerMove()
        {
            if(!_characterSystem.isMasterClient)
                return;

            if (_characterSystem.GetHP() > 0)
            {
                if (Get_Player_Move)
                    _characterSystem.StartMove();
                else
                    _characterSystem.StartAttack();
            }
            else
            {
                _characterSystem.StartPlayerDown();
            }
        }
        
        protected override float Move()
        {
            float inputMagnitude = base.Move();

            float[] speedData = new float[]{_animationBlend, inputMagnitude};
            
            if(_haspunPlayerController)
                _punPlayerController.SetSpeed(speedData, _punPlayerController.photonView.ViewID);
            return inputMagnitude;
        }
    }
}