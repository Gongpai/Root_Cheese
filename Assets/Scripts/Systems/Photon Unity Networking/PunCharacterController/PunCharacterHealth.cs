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

        public void TakeDamage(float amount, int OwnerNetID)
        {
            if (photonView != null)
                photonView.RPC("PunRPCApplyHealth", RpcTarget.MasterClient, -amount, OwnerNetID);
            else 
                print("photonView is NULL.");
        }

        public void HealingPoint(float amount, int OwnerNetID)
        {
            if (photonView != null) 
                photonView.RPC("PunRPCApplyHealth", photonView.Owner, amount, OwnerNetID);
            else 
                print("photonView is NULL.");
        }
        
        [PunRPC]
        public void PunRPCApplyHealth(float amount, int OwnerNetID)
        {
            Debug.Log("Update @" + PhotonNetwork.LocalPlayer.ActorNumber + " Apply Health : " + amount + " form : " +
                      OwnerNetID);
            
            if (_characterSystem.GetShield() > 0 && amount < 0)
            {
                photonView.RPC("PunRPCSetShield", photonView.Owner, amount, OwnerNetID);
                
                float shield_remaining = _characterSystem.GetShield() + amount;
                if(shield_remaining <= 0)
                    photonView.RPC("PunRPCSetHealth", photonView.Owner, shield_remaining, OwnerNetID);
            }
            else
            {
                photonView.RPC("PunRPCSetHealth", photonView.Owner, amount, OwnerNetID);

            }
            
            if (_characterSystem.GetHP() <= 0)
            {
                Debug.Log("NetID : " + OwnerNetID.ToString() + " Killed " + photonView.ViewID);
                photonView.RPC("PunResetPlayer", RpcTarget.All);
            }
        }

        [PunRPC]
        public void PunRPCSetShield(float amount, int OwnerNetID)
        {
            if (_characterSystem.GetShield() + amount > 0)
                _characterSystem.SetShiel(_characterSystem.GetShield() + amount);
            else
                _characterSystem.SetShiel(0);
            
            print($"Shield {gameObject.name} is : {_characterSystem.GetShield()}");
        }

        [PunRPC]
        public void PunRPCSetHealth(float amount, int OwnerNetID)
        {
            if (_characterSystem.GetHP() + amount > 0)
                _characterSystem.SetHP(_characterSystem.GetHP() + amount);
            else
                _characterSystem.SetHP(0);
            
            print($"Health {gameObject.name} is : {_characterSystem.GetHP()}");
        }

        [PunRPC]
        public void PunResetPlayer()
        {
            Debug.Log("Reset ...");
        }
    }
}