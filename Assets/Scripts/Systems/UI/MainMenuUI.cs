using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Play Panel")] 
        [SerializeField] private Button m_playButton;
        
        [Header("Option Panel")] 
        [SerializeField] private TextMeshProUGUI m_op;

        [Header("Player Account Panel")] 
        [SerializeField] private TextMeshProUGUI m_playerName;

        private GameManager GM;

        private void Start()
        {
            GM = GameManager.Instance;
            m_playerName.text = GM.playerInfo.playerName;
        }
    }
}