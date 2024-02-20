using GDD.StrategyPattern;
using Photon.Pun;
using UnityEngine;

namespace GDD
{
    public class EnemyState : CharacterStateMachine<EnemySystem>
    {
        protected IManeuverBehaviour<EnemyState> strategy;
        
        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }
        
        public override string StateName()
        {
            return "EnemyState";
        }

        public override void OnStart(EnemySystem contrller)
        {
            base.OnStart(contrller);
        }

        public override void Handle(EnemySystem contrller)
        {
            base.Handle(contrller);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public void ApplyEnemyStrategy()
        {
            strategy = GetComponent<EnemyManeuver>();
            //print("Apply Enemy Strategy");
            strategy.Maneuver(this, target);
        }

        public void WithdrawEnemyStrategy()
        {
            strategy.Truce();
        }
    }
}