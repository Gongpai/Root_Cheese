using System;
using System.Collections.Generic;
using GDD.PUN;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class FindRoomUI : MonoBehaviour
    {
        [SerializeField] private Button m_roomButton;
        [SerializeField] private TextMeshProUGUI _createRoomText;
        [SerializeField] private TextMeshProUGUI m_roomName;
        [SerializeField] private GameObject m_playerInfo;
        [SerializeField] private GameObject m_scrollViewContent;
        [SerializeField] private GameObject m_loadingUI;

        private PunNetworkManager PNM;
        private List<GameObject> _rooms = new List<GameObject>();
        private string _roomName;

        private void Start()
        {
            PNM = PunNetworkManager.Instance;
            PNM.OnRoomListUpdateAction = OnRoomListUpdate;
            PNM.OnJoinLobbyAction = OnJoinLobby;
            
            if(PhotonNetwork.InLobby)
                m_loadingUI.SetActive(false);
        }

        private void Update()
        {
            if (PhotonNetwork.InRoom)
                _createRoomText.text = "Leave Room";
            else
                _createRoomText.text = "Create Room";
        }

        private void OnJoinLobby()
        {
            m_loadingUI.SetActive(false);
            
            
        }

        private void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            if (_rooms.Count > 0)
            {
                foreach (var room in _rooms)
                {
                    Destroy(room);
                }

                _rooms = new List<GameObject>();
            }
            
            foreach (var roomInfo in roomList)
            {
                Canvas_Element_List f_object = Instantiate(m_playerInfo).GetComponent<Canvas_Element_List>();
                f_object.transform.parent = m_scrollViewContent.transform;
                f_object.transform.localScale = Vector3.one;
                f_object.transform.localPosition = Vector3.zero;

                if (roomInfo.Name == _roomName)
                    f_object.texts[0].text = $"{roomInfo.Name} - You";
                else
                    f_object.texts[0].text = roomInfo.Name;
                
                RoomInfoSlot f_slot = f_object.GetComponent<RoomInfoSlot>();
                f_slot.roomInfo = roomInfo;
                f_slot.buttonAction = OnJoinRoom;
                _rooms.Add(f_object.gameObject);
            }
        }

        public void EnterRoomName(string name)
        {
            _roomName = name;
        }

        public void JoinRoomButton()
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
                m_roomName.text = "CreateRoom ->";
            }
            else
            {
                PhotonNetwork.CreateRoom(_roomName);
                OnJoinRoom(_roomName);
            }
        }
        
        public void OnJoinRoom(string name)
        {
            m_roomName.text = name;
            
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.JoinRoom(name);
            }
            else
            {
                PhotonNetwork.JoinRoom(name);
            }
        }
    }
}