using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace GDD.PUN
{
    public class PunCharacterHealth : MonoBehaviourPun
    {
        private CharacterSystem _characterSystem;
        private byte _punEventCode = 10;

        private void Awake()
        {
            _characterSystem = GetComponent<CharacterSystem>();
        }

        private void OnEnable()
        {
            
        }

        private void Start()
        {
            //SyncPlayerStats
            photonView.RPC("GetPlayerStatsToOtherPlayer", RpcTarget.MasterClient);
        }
        
        [PunRPC]
        public void OnInitializeOtherPlayer(object[] datas, int OwnerNetID)
        {
            print($"{gameObject.name} | OnInitializeOtherPlayer");
            
            _characterSystem.SetHP((float)datas[0]);
            _characterSystem.SetMaxHP((float)datas[1]);
            _characterSystem.SetShield((float)datas[2]);
        }

        [PunRPC]
        public void GetPlayerStatsToOtherPlayer()
        {
            print($"GetPlayerStatsToOtherPlayer : {gameObject.name}");

            object[] datas = new object[]
            {
                _characterSystem.GetHP(),
                _characterSystem.GetMaxHP(),
                _characterSystem.GetShield(),
            };
            
            photonView.RPC("OnInitializeOtherPlayer", RpcTarget.Others, datas, photonView.ViewID);
        }
            
        public void TakeDamage(float amount, int OwnerNetID)
        {
            if (photonView != null)
                photonView.RPC("PunRPCApplyHealth", photonView.Owner, -amount, OwnerNetID);
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
                photonView.RPC("PunRPCSetShield", RpcTarget.All, amount, OwnerNetID);
                
                float shield_remaining = _characterSystem.GetShield() + amount;
                if(shield_remaining <= 0)
                    photonView.RPC("PunRPCSetHealth", RpcTarget.All, shield_remaining, OwnerNetID);
            }
            else
            {
                photonView.RPC("PunRPCSetHealth", RpcTarget.All, amount, OwnerNetID);

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
                _characterSystem.SetShield(_characterSystem.GetShield() + amount);
            else
                _characterSystem.SetShield(0);
            
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
        
        private void OnDisable()
        {
            
        }
    }
}