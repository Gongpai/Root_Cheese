using System.Collections;
using System.Linq;
using GDD.Spatial_Partition;
using Photon.Pun;
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
            
            //Get New Target
            GetNewTarget(contrller.SetTargetRandom());
            
            ApplyEnemyStrategy();
        }

        protected void GetNewTarget(int targetID)
        {
            if(GM.playMode == PlayMode.Singleplayer)
                target = GM.players.Keys.ElementAt(targetID).transform;
            else
            {
                print($"ID is : {targetID}");
                target = PhotonNetwork.GetPhotonView(targetID).transform;
            }
        }
        
        public override void Handle(EnemySystem contrller)
        {
            base.Handle(contrller);
            
            //print("Handleeeeeee");
            if(GM.players.Count > 0)
                transform.LookAt(target);
        }

        public override void OnExit()
        {
            base.OnExit();
            
            WithdrawEnemyStrategy();
        }
    }
}