using GDD.Spatial_Partition;

namespace GDD
{
    public class EnemySystem : CharacterSystem
    {
        private GameManager GM;
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
    }
}