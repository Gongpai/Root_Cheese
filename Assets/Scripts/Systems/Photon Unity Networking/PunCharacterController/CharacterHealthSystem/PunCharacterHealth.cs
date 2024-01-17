using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace GDD.PUN
{
    public class PunCharacterHealth : MonoBehaviourPun
    {
        protected CharacterSystem _characterSystem;
        protected byte _punEventCode = 10;

        public CharacterSystem CharacterSystem
        {
            get => _characterSystem;
        }
        
        protected virtual void Awake()
        {
            _characterSystem = GetComponent<CharacterSystem>();
        }

        protected virtual void OnEnable()
        {
            
        }

        protected virtual void Start()
        {
            //SyncPlayerStats
            photonView.RPC("GetPlayerStatsToOtherPlayer", RpcTarget.MasterClient);
        }

        protected virtual void Update()
        {
            
        }

        [PunRPC]
        public virtual void OnInitializeOtherPlayer(object[] datas, int OwnerNetID)
        {
            //print($"{gameObject.name} | OnInitializeOtherPlayer");
            
            _characterSystem.SetHP((float)datas[0]);
            _characterSystem.SetMaxHP((float)datas[1]);
            _characterSystem.SetShield((float)datas[2]);
            _characterSystem.SetEXP((int)datas[3]);
        }

        [PunRPC]
        public virtual void GetPlayerStatsToOtherPlayer()
        {
            //print($"GetPlayerStatsToOtherPlayer : {gameObject.name}");

            object[] datas = new object[]
            {
                _characterSystem.GetHP(),
                _characterSystem.GetMaxHP(),
                _characterSystem.GetShield(),
                _characterSystem.GetEXP()
            };
            
            photonView.RPC("OnInitializeOtherPlayer", RpcTarget.Others, datas, photonView.ViewID);
        }
            
        public virtual void TakeDamage(float amount, int OwnerNetID)
        {
            if (photonView != null)
                photonView.RPC("PunRPCApplyHealth", photonView.Owner, -amount, OwnerNetID);
            else 
                print("photonView is NULL.");
        }

        public virtual void HealingPoint(float amount, int OwnerNetID)
        {
            if (photonView != null) 
                photonView.RPC("PunRPCSetHealth", RpcTarget.All, amount, OwnerNetID);
            else 
                print("photonView is NULL.");
        }
        
        public virtual void ShieldPoint(float amount, int OwnerNetID)
        {
            if (photonView != null) 
                photonView.RPC("PunRPCSetShield", RpcTarget.All, amount, OwnerNetID);
            else 
                print("photonView is NULL.");
        }
        
        [PunRPC]
        public virtual void PunRPCApplyHealth(float amount, int OwnerNetID)
        {
            //Debug.Log("Update @" + PhotonNetwork.LocalPlayer.ActorNumber + " Apply Health : " + amount + " form : " + OwnerNetID);
            
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
                //Debug.Log("NetID : " + OwnerNetID.ToString() + " Killed " + photonView.ViewID);
                photonView.RPC("PunResetCharacter", RpcTarget.All);
            }
        }

        [PunRPC]
        public virtual void PunRPCSetShield(float amount, int OwnerNetID)
        {
            if (_characterSystem.GetShield() + amount > 0)
                _characterSystem.SetShield(_characterSystem.GetShield() + amount);
            else
                _characterSystem.SetShield(0);
            
            print($"Shield {gameObject.name} is : {_characterSystem.GetShield()}");
        }

        [PunRPC]
        public virtual void PunRPCSetHealth(float amount, int OwnerNetID)
        {
            if (_characterSystem.GetHP() + amount > 0)
                _characterSystem.SetHP(_characterSystem.GetHP() + amount);
            else
                _characterSystem.SetHP(0);
            
            print($"Health {gameObject.name} is : {_characterSystem.GetHP()}");
        }

        [PunRPC]
        public virtual void PunResetCharacter()
        {
            Debug.Log("Reset ...");
        }
        
        protected virtual void OnDisable()
        {
            
        }
    }
}