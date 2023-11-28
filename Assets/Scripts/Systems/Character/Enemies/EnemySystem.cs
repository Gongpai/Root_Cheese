using GDD.Spatial_Partition;
using UnityEngine;

namespace GDD
{
    public class EnemySystem : CharacterSystem
    {
        private GameManager GM;
        private Vector2Int cellPos = new Vector2Int();
        
        public override void Start()
        {
            base.Start();
            GM = GameManager.Instance;
            
            //Add this unit to the grid
            GM.grid.Add(this);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            
            if (Application.isPlaying)
            {
                GM.grid.Remove(cellPos);
                print("Cell Pos : " + cellPos);
            }
        }

        public override Vector2Int GetCellPosition()
        {
            return cellPos;
        }

        public override void SetCellPosition(Vector2Int cell)
        {
            print("Cell Pos : " + cell);
            cellPos = cell;
        }
    }
}