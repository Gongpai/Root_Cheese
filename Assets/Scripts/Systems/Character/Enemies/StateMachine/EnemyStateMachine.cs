using System;
using GDD;
using GDD.StrategyPattern;

namespace GDD
{
    public class EnemyStateMachine : CharacterStateMachine<EnemySystem>
    {
        protected IManeuverBehaviour<EnemyStateMachine> strategy;
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

        public void ApplyEnemyStrategy()
        {
            strategy = GetComponent<EnemyManeuver>();
            print("Apply Enemy Strategy");
            strategy.Maneuver(this);
        }

        public void WithdrawEnemyStrategy()
        {
            strategy.Truce();
        }
    }
}