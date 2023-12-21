using System;
using GDD.PUN;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GDD
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PunPlayerCharacterController))]
    public class MultiplayerCharacterController : PlayerCharacterController
    {
        private PunPlayerCharacterController _punPlayerController;
        private bool _haspunPlayerController;

        protected override void Start()
        {
            base.Start();

            _haspunPlayerController = TryGetComponent(out _punPlayerController);
            _punPlayerController = GetComponent<PunPlayerCharacterController>();
            _punPlayerController.footStepAudioClipLists = _footStepAudioClipLists;
        }

        protected override float Move()
        {
            float inputMagnitude = base.Move();

            float[] speedData = new float[]{_animationBlend, inputMagnitude};
            _punPlayerController.SetSpeed(speedData, _punPlayerController.photonView.ViewID);
            return inputMagnitude;
        }
    }
}