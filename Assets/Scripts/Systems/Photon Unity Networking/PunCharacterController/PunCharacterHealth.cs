using System;
using Photon.Pun;
using UnityEngine;

namespace GDD.PUN
{
    public class PunCharacterHealth : MonoBehaviourPun
    {
        private CharacterSystem _characterSystem;

        private void Start()
        {
            _characterSystem = GetComponent<CharacterSystem>();
        }

        public void TakeDamage(int amount, int OwnerNetID)
        {
            if (photonView != null)
                photonView.RPC("PunRPCApplyHealth", RpcTarget.MasterClient, amount * -1, OwnerNetID);
            else 
                print("photonView is NULL.");
        }

        public void HealingPoint(int amount, int OwnerNetID)
        {
            if (photonView != null) 
                photonView.RPC("PunRPCApplyHealth", photonView.Owner, amount, OwnerNetID);
            else 
                print("photonView is NULL.");
        }
        
        [PunRPC]
        public void PunRPCApplyHealth(int amount, int OwnerNetID)
        {
            Debug.Log("Update @" + PhotonNetwork.LocalPlayer.ActorNumber + " Apply Health : " + amount + " form : " +
                      OwnerNetID);
            
            if (_characterSystem.GetShield() > 0 && amount < 0)
            {
                if (_characterSystem.GetShield() + amount > 0)
                    _characterSystem.SetShiel(_characterSystem.GetShield() + amount);
                else
                    _characterSystem.SetShiel(0);
            }
            else
            {
                if (_characterSystem.GetHP() + amount > 0)
                    _characterSystem.SetHP(_characterSystem.GetHP() + amount);
                else
                    _characterSystem.SetHP(0);
            }
            
            if (_characterSystem.GetHP() <= 0)
            {
                Debug.Log("NetID : " + OwnerNetID.ToString() + " Killed " + photonView.ViewID);
                photonView.RPC("PunResetPlayer", RpcTarget.All);
            }
        }

        [PunRPC]
        public void PunResetPlayer()
        {
            Debug.Log("Reset ...");
        }
    }
}