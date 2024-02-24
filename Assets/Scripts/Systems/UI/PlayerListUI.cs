using System;
using System.Collections.Generic;
using System.Linq;
using GDD.PUN;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class PlayerListUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_friendsSlotGroup;
        [SerializeField] private GameObject m_friendsSlot;
        [SerializeField] private Button m_readyButton;

        private PunNetworkManager PNM;
        GameManager GM;
        private List<Player> _playerList;
        private List<GameObject> _friendsSlot = new List<GameObject>();

        private void OnEnable()
        {
            PNM = PunNetworkManager.Instance;
            PNM.OnPlayerListUpdateAction += OnPlayerListUpdate;
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

                int iPlayer = i;
                f_object.buttons[0].onClick.AddListener(() =>
                {
                    PNM.OnPlayerListUpdateAction?.Invoke(new List<Player>());
                    PhotonNetwork.CloseConnection(playerList[iPlayer]);
                });

                PlayerInfoSlot _playerInfoSlot = f_object.GetComponent<PlayerInfoSlot>();
                _playerInfoSlot.iPlayer = i;

                _friendsSlot.Add(f_object.gameObject);
            }
        }

        public void ReadyButton()
        {
            GM.players.Keys.ElementAt(0).ReadyButton();
        }

        private void OnDisable()
        {
            PNM.OnPlayerListUpdateAction -= OnPlayerListUpdate;
        }
    }
}