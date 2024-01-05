using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace GDD.PUN
{
    public class PunRoomManager : MonoBehaviourPunCallbacks
    {
        public delegate void HasGameState();
        public static event HasGameState OnEndGame;
        public static event HasGameState OnStartGame;

        enum gameState
        {
            Start,
            End
        }

        public override void OnEnable()
        {
            base.OnEnable();
        }

        private void Start()
        {
            OnStartGame += (() => { print("Startttttttttttttttttttttt!!!!------"); });
            OnEndGame += (() => { print("Endddddddddddddddddddddddddd!!!!------"); });
            
            ChangeGameState(gameState.Start);
        }

        private void Update()
        {
            
        }

        private void ChangeGameState(gameState _gameState)
        {
            Hashtable props = new Hashtable
            {
                { PunGameSetting.GAMESTATE, _gameState }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
            
            print("Startttttttttttttttt");
            //Game State
                OnGameState(targetPlayer, changedProps);
            
            //Pre-Random Position Target Enemy
            
        }

        public void OnGameState(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey(PunGameSetting.GAMESTATE) &&
                targetPlayer.ActorNumber == photonView.ControllerActorNr)
            {
                object state;
                if (changedProps.TryGetValue(PunGameSetting.GAMESTATE, out state))
                {
                    switch ((gameState)state)
                    {
                        case gameState.Start:
                            print("Startttttttttttttttt");
                            OnStartGame.Invoke();
                            break;
                        case gameState.End:
                            print("Endddddddddddddddddd");
                            OnEndGame.Invoke();
                            break;
                    }
                }
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();
            
            ChangeGameState(gameState.End);
        }
    }
}