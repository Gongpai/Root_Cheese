using GDD.Spatial_Partition;
using UnityEngine;

namespace GDD
{
    public class PlayerStateMachine : StateMachine<PlayerSystem>
    {
        protected PlayerSystem _playerSystem;
        protected GameManager GM;
        protected Transform target;
        
        protected virtual void Start()
        {
            GM = GameManager.Instance;
            _playerSystem = GetComponent<PlayerSystem>();
        }
        
        public override string StateName()
        {
            return "State";
        }
        
        public override void OnStart(PlayerSystem contrller)
        {
            base.OnStart(contrller);
        }

        public override void Handle(PlayerSystem contrller)
        {
            base.Handle(contrller);
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}