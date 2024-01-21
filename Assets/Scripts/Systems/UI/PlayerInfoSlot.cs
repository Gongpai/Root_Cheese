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
            if(GM.players.Count <= 0 && _iPlayer > GM.players.Values.Count)
                return;
            
            if (GM.players.Values.ElementAt(_iPlayer))
                m_status.sprite = m_ready;
            else 
                m_status.sprite = m_notReady;
        }
    }
}