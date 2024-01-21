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
            if(!photonView.IsMine)
                photonView.RPC("GetPlayerStatsToOtherPlayer", photonView.Owner, photonView.ViewID);
        }

        protected virtual void Update()
        {
            
        }

        [PunRPC]
        public virtual void OnInitializeOtherPlayer(object[] datas, int OwnerNetID)
        {
            if(OwnerNetID != photonView.ViewID || photonView.IsMine)
                return;
            
            print($"{gameObject.name} | OnInitializeOtherPlayer || Shield = {(float)datas[2]}");
            print($"Shield = {(float)datas[2]}");
            
            _characterSystem.SetHP((float)datas[0]);
            _characterSystem.SetMaxHP((float)datas[1]);
            _characterSystem.SetShield((float)datas[2]);
            _characterSystem.SetUpdateEXP((int)datas[3]);
            _characterSystem.SetEXP((int)datas[3]);
        }

        [PunRPC]
        public virtual void GetPlayerStatsToOtherPlayer(object OwnerNetID)
        {
            if((int)OwnerNetID != photonView.ViewID)
                return;
            
            print($"GetPlayerStatsToOtherPlayer : {gameObject.name} || ID : {OwnerNetID} = {photonView}");

            float shield;
            if (photonView.IsMine)
                shield = GameManager.Instance.gameInstance.shield;
            else
                shield = _characterSystem.GetShield();
            
            print($"Shieldddddddddd ::::: {shield}");
            
            object[] datas = new object[]
            {
                _characterSystem.GetHP(),
                _characterSystem.GetMaxHP(),
                shield,
                _characterSystem.GetUpdateEXP()
            };
            
            photonView.RPC("OnInitializeOtherPlayer", RpcTarget.All, datas, OwnerNetID);
        }
            
        public virtual void TakeDamage(float amount)
        {
            if (photonView != null)
                photonView.RPC("PunRPCApplyHealth", photonView.Owner, -amount, photonView.ViewID);
            else 
                print("photonView is NULL.");
        }

        public virtual void HealingPoint(float amount)
        {
            if (photonView != null) 
                photonView.RPC("PunRPCSetHealth", RpcTarget.All, amount, photonView.ViewID);
            else 
                print("photonView is NULL.");
        }
        
        public virtual void ShieldPoint(float amount)
        {
            if (photonView != null) 
                photonView.RPC("PunRPCSetShield", RpcTarget.All, amount, photonView.ViewID);
            else 
                print("photonView is NULL.");
        }
        
        [PunRPC]
        public virtual void PunRPCApplyHealth(float amount, int OwnerNetID)
        {
            if(OwnerNetID != photonView.ViewID)
                return;
            
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
            print($"Shield {gameObject.name} is : {amount}");
            if(OwnerNetID != photonView.ViewID)
                return;
            
            if (_characterSystem.GetShield() + amount > 0)
                _characterSystem.SetShield(_characterSystem.GetShield() + amount);
            else
                _characterSystem.SetShield(0);
        }

        [PunRPC]
        public virtual void PunRPCSetHealth(float amount, int OwnerNetID)
        {
            if(OwnerNetID != photonView.ViewID)
                return;
            
            print($"HP Amount = {amount}");
            print($"Total HP = {_characterSystem.GetHP() + amount} || Char HP = {_characterSystem.GetHP()}");
            
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