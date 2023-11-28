using GDD.Spatial_Partition;
using UnityEngine;

namespace GDD
{
    public class EnemySystem : CharacterSystem
    {
        private GameManager GM;
        private Vector3 oldPos;
        
        public override void Start()
        {
            base.Start();
            GM = GameManager.Instance;
            
            //Add this unit to the grid
            GM.grid.Add(this);

            oldPos = transform.position;
        }

        public override void Update()
        {
            base.Update();

            //UpdateEnemyMove();
        }

        private void UpdateEnemyMove()
        {
            //Add this unit to the grid
            GM.grid.OnPawnMove(this, oldPos);

            //Init the old pos
            oldPos = transform.position;
        }
        
        public override void OnDisable()
        {
            base.OnDisable();
            
            if (Application.isPlaying)
            {
                GM.grid.Remove(cellPos, this);
                //print("Cell Pos : " + cellPos);
            }
        }
    }
}