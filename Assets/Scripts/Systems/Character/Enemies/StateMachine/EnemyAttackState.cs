using System.Collections;
using GDD.Spatial_Partition;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public class EnemyAttackState : EnemyState
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
            
            //print("Handleeeeeee");
            transform.LookAt(GM.players[0].transform);
        }

        public override void OnExit()
        {
            base.OnExit();
            
            WithdrawEnemyStrategy();
        }
    }
}