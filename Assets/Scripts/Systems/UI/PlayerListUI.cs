using System;
using System.Collections.Generic;
using System.Linq;
using GDD.PUN;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class PlayerListUI : MonoBehaviour
    {
        [SerializeField] private string m_ready = "Ready?";
        [SerializeField] private string m_wait = "Wait!";
        [SerializeField] private GameObject m_friendsSlotGroup;
        [SerializeField] private GameObject m_friendsSlot;
        [SerializeField] private TextMeshProUGUI m_readyText;
        [SerializeField] private Button m_readyButton;

        private PunNetworkManager PNM;
        GameManager GM;
        private List<Player> _playerList;
        private List<GameObject> _friendsSlot = new List<GameObject>();
        private int myID;

        private void OnEnable()
        {
            PNM = PunNetworkManager.Instance;
            PNM.OnPlayerListUpdateAction += OnPlayerListUpdate;
            PNM.OnLeftRoomAction += OnLeftRoom;
        }

        private void Start()
        {
            GM = GameManager.Instance;
            
            if(_friendsSlot.Count <= 0)
                PNM.OnReCheckPlayerRoom();
        }

        private void OnPlayerListUpdate(List<Player> playerList)
        {
            if (_friendsSlot.Count > 0)
            {
                foreach (var friends in _friendsSlot)
                {
                    Destroy(friends);
                }

                _friendsSlot = new List<GameObject>();
            }

            _playerList = playerList;
            m_friendsSlotGroup.SetActive(playerList.Count > 0);
            m_readyButton.gameObject.SetActive(playerList.Count > 0);
            for (int i = 0; i < playerList.Count; i++)
            {
                Canvas_Element_List f_object = Instantiate(m_friendsSlot).GetComponent<Canvas_Element_List>();
                f_object.transform.parent = m_friendsSlotGroup.transform;
                f_object.transform.localScale = Vector3.one;
                f_object.transform.localPosition = Vector3.zero;
                f_object.texts[0].text = playerList[i].NickName;
                
                print($"Player List {playerList[i]} : {PhotonNetwork.LocalPlayer}");
                if (playerList[i] == PhotonNetwork.LocalPlayer)
                    myID = i;

                if (PhotonNetwork.IsMasterClient)
                {
                    int iPlayer = i;
                    f_object.buttons[0].onClick.AddListener(() =>
                    {
                        PNM.OnPlayerListUpdateAction?.Invoke(new List<Player>());
                        PhotonNetwork.CloseConnection(playerList[iPlayer]);
                    });
                }
                else
                {
                    f_object.buttons[0].gameObject.SetActive(false);
                }
                
                PlayerInfoSlot _playerInfoSlot = f_object.GetComponent<PlayerInfoSlot>();
                _playerInfoSlot.iPlayer = i;

                _friendsSlot.Add(f_object.gameObject);
            }
        }

        public void OnLeftRoom()
        {
            m_readyText.text = m_ready;
        }

        public void ReadyButton()
        {
            KeyValuePair<PlayerSystem, bool> player = GM.players.ElementAt(myID);
            player.Key.ReadyButton();
            SetReadyText(player.Value);
        }

        private void SetReadyText(bool isReady)
        {
            m_readyText.text = isReady ? m_ready : m_wait;
        }

        private void OnDisable()
        {
            PNM.OnPlayerListUpdateAction -= OnPlayerListUpdate;
            PNM.OnLeftRoomAction -= OnLeftRoom;
        }
    }
}