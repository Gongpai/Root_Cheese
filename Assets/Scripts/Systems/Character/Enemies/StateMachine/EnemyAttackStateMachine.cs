using System.Collections;
using GDD.Spatial_Partition;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public class EnemyAttackStateMachine : EnemyStateMachine
    {
        public override string StateName()
        {
            return "EnemyAttackState";
        }
        
        public override void OnStart(EnemySystem contrller)
        {
            base.OnStart(contrller);

            ApplyEnemyStrategy();
        }
        
        public override void Handle(EnemySystem contrller)
        {
            base.Handle(contrller);
            
            
        }

        public override void OnExit()
        {
            base.OnExit();
            
            WithdrawEnemyStrategy();
        }
    }
}