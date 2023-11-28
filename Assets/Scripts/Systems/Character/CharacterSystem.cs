using System;
using GDD.Spatial_Partition;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GDD
{
    public abstract class CharacterSystem : MonoBehaviour, IPawn
    {
        [SerializeField] protected Slider m_hp_bar;
        [SerializeField] protected float m_hp = 100;
        
        IPawn previousPawn;
        private IPawn nextPawn;

        public float hp
        {
            get => m_hp;
            set => m_hp = value;
        }

        public virtual void Start()
        {
            
        }

        public virtual void Update()
        {
            m_hp_bar.value = m_hp / 100;
            
            if(m_hp <= 0)
                Destroy(gameObject);
        }

        public virtual void OnDisable()
        {
            
        }

        public virtual Vector2Int GetCellPosition()
        {
            return new Vector2Int();
        }

        public virtual void SetCellPosition(Vector2Int cell)
        {
            
        }

        public virtual Vector2 GetPawnVision()
        {
            return new Vector2();
        }
        public virtual void SetPawnVision(Vector2 vision)
        {
            
        }

        public Transform GetPawnTransform()
        {
            return transform;
        }

        public IPawn GetPreviousPawn()
        {
            return previousPawn;
        }

        public void SetPreviousPawn(IPawn pawn)
        {
            previousPawn = pawn;
        }

        public IPawn GetNextPawn()
        {
            return nextPawn;
        }

        public void SetNextPawn(IPawn pawn)
        {
            nextPawn = pawn;
        }
    }
}