using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GDD
{
    public class RoomInfoSlot : MonoBehaviour
    {
        [SerializeField] private Image m_status;
        private RoomInfo _roomInfo;
        private Button _button;
        private UnityAction<string> _buttonAction;
        private bool _isRoomFull;

        public UnityAction<string> buttonAction
        {
            get => _buttonAction;
            set => _buttonAction = value;
        }

        public RoomInfo roomInfo
        {
            get => _roomInfo;
            set => _roomInfo = value;
        }

        public bool isRoomFull
        {
            get => _isRoomFull;
            set => _isRoomFull = value;
        }

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() =>
            {
                _buttonAction.Invoke(_roomInfo.Name);
            });
        }

        private void Update()
        {
            if (_isRoomFull)
                m_status.color = new Color(1, 0.5f, 0, 1);
            else
                m_status.color = Color.green;
        }
    }
}