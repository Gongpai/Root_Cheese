using System;
using GDD.PUN;
using UnityEngine;

namespace GDD
{
    [RequireComponent(typeof(PunPlayerCharacterController))]
    public class MultiplayerAttackState : PlayerAttackState
    {
        private PunPlayerCharacterController _punPlayerController;
        private bool _haspunPlayerController;

        protected override void Start()
        {
            base.Start();
            
            _haspunPlayerController = TryGetComponent(out _punPlayerController);

            if (_haspunPlayerController)
            {
                print($"Weapon System {_weaponSystem == null}");
                _punPlayerController.weaponSystem = _weaponSystem;
                _punPlayerController.playerSpawnBullet = PlayerSpawnBullet;
            }

        }

        protected override void ToggleFire()
        {
            base.ToggleFire();
            
            _punPlayerController.CallRaiseToggleFireEvent();
        }
    }
}