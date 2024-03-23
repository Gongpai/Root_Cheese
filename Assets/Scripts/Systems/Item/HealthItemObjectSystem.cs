using UnityEngine;

namespace GDD
{
    public class HealthItemObjectSystem : EnemySystem
    {
        private Animator _animator;
        
        public override void Awake()
        {
            GM = GameManager.Instance;
            _animator = GetComponent<Animator>();
        }

        public override void OnEnable()
        {
            
        }

        public override void Start()
        {
            AddObjectToGrid();
        }

        public override void Update()
        {
            m_hp_bar.value = GetHP() / GetMaxHP();
            
            if (GetHP() <= 0 && !_isDead)
            {
                OnCharacterDead();
            }
        }

        private void UpdateEnemyMove()
        {
            //Add this unit to the grid
            GM.grid.OnPawnMove(this, oldPos);

            //Init the old pos
            oldPos = transform.position;
        }

        public void AddObjectToGrid()
        {
            GM.enemies.Add(this);
            GM.grid.Add(this);
            
            UpdateEnemyMove();
        }

        public override void OnCharacterDead()
        {
            _animator.enabled = true;
        }

        public override void OnDisable()
        {
            
        }

        public override void OnDestroy()
        {
            GM.grid.Remove(cellPos, this);
            GM.enemies.Remove(this);
        }
    }
}