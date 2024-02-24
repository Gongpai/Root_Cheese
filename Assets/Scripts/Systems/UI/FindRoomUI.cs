using System;
using System.Collections.Generic;
using System.Linq;
using GDD.PUN;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GDD
{
    public class FindRoomUI : MonoBehaviour
    {
        [SerializeField] private Button m_createRoomButton;
        [SerializeField] private TextMeshProUGUI _createRoomText;
        [SerializeField] private TextMeshProUGUI m_roomName;
        [SerializeField] private GameObject m_playerInfo;
        [SerializeField] private GameObject m_scrollViewContent;
        [SerializeField] private GameObject m_loadingUI;
        [SerializeField] private int m_maxPlayer = 2;
        [SerializeField] private UnityEvent m_OpenCreateRoomUIEvent;

        private PunNetworkManager PNM;
        private List<GameObject> _rooms = new List<GameObject>();
        private List<RoomInfo> _roomList = new List<RoomInfo>();
        private Tuple<RoomInfo, GameObject> currentRoomInfo;
        private string _currentRoomName;

        private string _roomName;

        private void OnEnable()
        {
            PNM = PunNetworkManager.Instance;
            PNM.OnRoomListUpdateAction += OnRoomListUpdate;
            PNM.OnJoinLobbyAction += OnJoinLobby;
            PNM.OnJoinConnectToMasterAction += OnJoinConnectedToMaster;
            PNM.OnJoinRoomAction += OnJoinRoomCallBack;
            PNM.OnJoinRoomFailedAction += OnJoinRoomFailedCallBack;
            PNM.OnPlayerEnteredRoomAction += OnPlayerEnteredRoomCallBack;
            
            PNM.OnReUpdateRoomList();
        }

        private void Start()
        {
            if(PhotonNetwork.InLobby)
                m_loadingUI.SetActive(false);
        }

        private void Update()
        {
            if (PhotonNetwork.InRoom)
                _createRoomText.text = "Leave Room";
            else
                _createRoomText.text = "Create Room";
            
            /*
            if(currentRoomInfo != null)
                print($"Room Info = {currentRoomInfo.PlayerCount}/{currentRoomInfo.MaxPlayers}");*/
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

            _roomList = roomList;
            for (int i = 0; i < roomList.Count; i++)
            {
                RoomInfo roomInfo = roomList[i];
                Canvas_Element_List f_object = Instantiate(m_playerInfo).GetComponent<Canvas_Element_List>();
                f_object.transform.parent = m_scrollViewContent.transform;
                f_object.transform.localScale = Vector3.one;
                f_object.transform.localPosition = Vector3.zero;
                
                RoomInfoSlot f_slot = f_object.GetComponent<RoomInfoSlot>();
                if (PhotonNetwork.InRoom && roomInfo.Name == PhotonNetwork.CurrentRoom.Name)
                {
                    f_object.texts[0].text = $"{roomInfo.Name} - You";
                    f_slot.isRoomFull = true;
                }
                else
                {
                    f_object.texts[0].text = roomInfo.Name;
                    f_slot.isRoomFull = false;
                }

                f_slot.roomInfo = roomInfo;
                f_slot.buttonAction = _name =>
                {
                    OnJoinRoom(_name);
                };
                _rooms.Add(f_object.gameObject);
            }
        }

        private void OnJoinConnectedToMaster()
        {
            m_loadingUI.SetActive(false);
        }
        
        private void OnJoinRoomCallBack()
        {
            m_loadingUI.SetActive(false);
            OnRoomListUpdate(_roomList);
        }

        void OnPlayerEnteredRoomCallBack(Room _room)
        {
            
        }
        
        public void EnterRoomName(string name)
        {
            _roomName = name;
        }

        public void OpenRoomUIAndLeaveRoom()
        {
            if (PhotonNetwork.InRoom)
            {
                print($"Leave Room!");
                PNM.OnPlayerListUpdateAction?.Invoke(new List<Player>());
                
                PhotonNetwork.LeaveRoom();
                m_roomName.text = "Room :";
                _roomName = "";
                
                OnRoomListUpdate(_roomList);
            }
            else
            {
                m_OpenCreateRoomUIEvent?.Invoke();
            }
        }

        public void CreateRoomButton()
        {
            print($"Join Room!");
            RoomOptions _roomOptions = new RoomOptions();
            _roomOptions.MaxPlayers = m_maxPlayer;
            PhotonNetwork.CreateRoom(_roomName, _roomOptions, TypedLobby.Default);
            m_roomName.text = $"Room : {_roomName}";
            m_loadingUI.SetActive(true);
            OnRoomListUpdate(_roomList);
        }

        private void OnJoinRoom(string name)
        {
            if (!PhotonNetwork.IsConnectedAndReady)
                return;

            m_loadingUI.SetActive(true);
            
            m_roomName.text = $"Room : {name}";
            _roomName = name;
            
            PhotonNetwork.JoinRoom(name);
            OnRoomListUpdate(_roomList);
        }

        private void OnJoinRoomFailedCallBack(short returnCode, string message)
        {
            m_loadingUI.SetActive(false);
        }

        private void OnDisable()
        {
            PNM.OnRoomListUpdateAction -= OnRoomListUpdate;
            PNM.OnJoinLobbyAction -= OnJoinLobby;
            PNM.OnJoinConnectToMasterAction -= OnJoinConnectedToMaster;
            PNM.OnJoinRoomAction -= OnJoinRoomCallBack;
            PNM.OnJoinRoomFailedAction -= OnJoinRoomFailedCallBack;
            PNM.OnPlayerEnteredRoomAction -= OnPlayerEnteredRoomCallBack;
            m_loadingUI.SetActive(false);
        }
    }
}