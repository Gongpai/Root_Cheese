using GDD.PUN;
using UnityEngine;

namespace GDD
{
    public class MultiplayerEnemyController : EnemyControllerSystem
    {
        private PunEnemyCharacterController _punEnemyController;
        private bool _haspunEnemyController;
        
        protected override void Start()
        {
            base.Start();

            _haspunEnemyController = TryGetComponent(out _punEnemyController);
            _punEnemyController = GetComponent<PunEnemyCharacterController>();
            
            if(_haspunEnemyController)
                _punEnemyController.footStepAudioClipLists = _footStepAudioClipLists;
        }

        public override float Move(Vector3 AgentDestination, float AgentSpeed)
        {
            float inputMagnitude = base.Move(AgentDestination, AgentSpeed);

            float[] speedData = new float[]{_animationBlend, inputMagnitude};
            
            if(_haspunEnemyController)
                _punEnemyController.SetSpeed(speedData, _punEnemyController.photonView.ViewID);
            return inputMagnitude;
        }
    }
}