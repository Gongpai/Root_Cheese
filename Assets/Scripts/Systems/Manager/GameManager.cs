using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GDD.Sinagleton;
using GDD.Spatial_Partition;
using UnityEngine;
using Grid = GDD.Spatial_Partition.Grid;

namespace GDD
{
    public class GameManager : DontDestroy_Singleton<GameManager>
    {
        [Header("Emeny Finding System")] 
        [SerializeField]
        private List<PlayerSystem> m_players;
        [SerializeField]
        private List<EnemySystem> m_enemies;
        [SerializeField]
        private float m_mapWidth = 50f;
        [SerializeField]
        private int m_cellSize = 10;
        
        private GameObject _bullet_pool;
        private Grid _grid;
        
        public GameObject Get_Bullet_Pool
        {
            get
            {
                if (_bullet_pool == null)
                {
                    _bullet_pool = new GameObject("Bullet_Pool");
                    _bullet_pool.transform.position = Vector3.zero;
                }

                return _bullet_pool;
            }
        }

        public Grid grid
        {
            get => _grid;
        }
        
        public List<PlayerSystem> players
        {
            get => m_players;
        }

        public List<EnemySystem> enemies
        {
            get => m_enemies;
        }

        public override void OnAwake()
        {
            base.OnAwake();
            
            m_players = FindObjectsByType<PlayerSystem>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList();
            m_enemies = FindObjectsByType<EnemySystem>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList();
            _grid = new Grid((int)m_mapWidth, m_cellSize, enemies.Count);
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}