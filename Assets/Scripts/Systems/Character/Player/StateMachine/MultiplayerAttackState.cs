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

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _haspunPlayerController = TryGetComponent(out _punPlayerController);
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void ToggleFire()
        {
            base.ToggleFire();
            
            _punPlayerController.CallRaiseToggleFireEvent(GM.enemies.IndexOf(closestEnemy.GetPawnTransform().GetComponent<EnemySystem>()));
        }
    }
}