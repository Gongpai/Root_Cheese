using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class PlayerInfoSlot : MonoBehaviour
    {
        [SerializeField] private Sprite m_notReady;
        [SerializeField] private Sprite m_ready;
        [SerializeField] private Image m_status;
        [SerializeField] private Image m_border;
        private int _iPlayer;
        private GameManager GM;

        public int iPlayer
        {
            get => _iPlayer;
            set => _iPlayer = value;
        }

        private void Start()
        {
            GM = GameManager.Instance;
        }

        private void Update()
        {
            if(GM.players.Count <= 0 || _iPlayer > GM.players.Values.Count - 1)
                return;

            if (GM.players.Values.ElementAt(_iPlayer))
            {
                m_status.sprite = m_ready;
                m_status.color = Color.green;
                m_border.color = Color.green;
            }
            else
            {
                m_status.sprite = m_notReady;
                m_status.color = Color.red;
                m_border.color = Color.white;
            }
        }
    }
}