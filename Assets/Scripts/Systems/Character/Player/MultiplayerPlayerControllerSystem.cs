using System;
using GDD.PUN;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GDD
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PunPlayerCharacterController))]
    public class MultiplayerPlayerControllerSystem : CharacterControllerSystem
    {
        private PunPlayerCharacterController _punPlayerController;
        private bool _haspunPlayerController;

        protected override void Start()
        {
            base.Start();

            _haspunPlayerController = TryGetComponent(out _punPlayerController);
            _punPlayerController = GetComponent<PunPlayerCharacterController>();
            
            if(_haspunPlayerController)
                _punPlayerController.footStepAudioClipLists = _footStepAudioClipLists;
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