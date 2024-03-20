using System;
using Supabase.Realtime.Channel;
using UnityEngine;

namespace GDD
{
    public class HealthItem : MonoBehaviour
    {
        [SerializeField] private float m_HP = 50;
        private DropItemObjectPool _dropItemObject;
        private GameManager GM;
        private HealthSystem _healthSystem;
        private bool _isDrop;
        private void Start()
        {
            GM = GameManager.Instance;
            _healthSystem = GetComponent<HealthSystem>();
            _dropItemObject = GetComponent<DropItemObjectPool>();
        }

        private void Update()
        {
            
        }

        public void OnDropItem()
        {
            _dropItemObject.OnCreateObject();
        }

        public void AddHP()
        {
            foreach (var players in GM.players.Keys)
            {
                players.SetHP(players.GetHP() + m_HP);
            }
        }

        public void RemoveObject()
        {
            Destroy(gameObject);
        }
    }
}