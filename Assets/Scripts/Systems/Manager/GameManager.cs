using System;
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
        private Transform m_player_layer;
        [SerializeField]
        private List<EnemySystem> m_enemies;
        [SerializeField]
        private Transform m_enemy_layer;
        [SerializeField]
        private float m_mapWidth = 50f;
        [SerializeField]
        private int m_cellSize = 10;

        [Header("Play Mode")] 
        [SerializeField] 
        private PlayMode m_playMode;
        
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

        public Transform player_layer
        {
            get => m_player_layer;
        }

        public List<EnemySystem> enemies
        {
            get => m_enemies;
        }
        
        public Transform enemy_layer
        {
            get => m_enemy_layer;
        }

        public float mapWidth
        {
            get => m_mapWidth;
        }

        public PlayMode playMode
        {
            get => m_playMode;
        }

        public override void OnAwake()
        {
            base.OnAwake();
            
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

        private void AddNewPlayer()
        {
            
        }
        
        void OnDrawGizmosSelected()
        {
            foreach (var _player in players)
            {
                Vector2 p_vision = _player.GetComponent<PlayerSystem>().Get_Vision / 2;
                //Determine which grid cell the friendly soldier is in
                float cellX = _player.GetPawnTransform().position.x;
                float cellZ = _player.GetPawnTransform().position.z;
                Vector2[] cells_pos = new Vector2[4];
                cells_pos[0] = new Vector2(cellX + p_vision.x, cellZ - p_vision.y);
                cells_pos[1] = new Vector2(cellX + p_vision.x, cellZ + p_vision.y);
                cells_pos[2] = new Vector2(cellX - p_vision.x, cellZ + p_vision.y);
                cells_pos[3] = new Vector2(cellX - p_vision.x, cellZ - p_vision.y);
                
                Gizmos.color = new Color(0, 1, 0, 0.5f);
                Gizmos.DrawCube(_player.GetPawnTransform().position, new Vector3(_player.Get_Vision.x, 1, _player.Get_Vision.y));
                
                
                int[,] cells_pos_map = new int[4, 2]
                {
                    { (int)((cellX + p_vision.x) / m_cellSize), (int)((cellZ - p_vision.y) / m_cellSize)},
                    { (int)((cellX + p_vision.x) / m_cellSize), (int)((cellZ + p_vision.y) / m_cellSize)},
                    { (int)((cellX - p_vision.x) / m_cellSize), (int)((cellZ + p_vision.y) / m_cellSize)},
                    { (int)((cellX - p_vision.x) / m_cellSize), (int)((cellZ - p_vision.y) / m_cellSize)}
                };
                
                Gizmos.color = new Color(0, 0, 1, 0.5f);
                for (int i = 0; i < cells_pos_map.Length / 2; i++)
                {
                    Gizmos.DrawCube(new Vector3(cells_pos_map[i, 0] * m_cellSize + (5f), 0, cells_pos_map[i, 1] * m_cellSize + (5f)), new Vector3(1, 1, 1));
                }
                
                Gizmos.color = new Color(1, 0, 0, 0.5f);
                int _i = 1;
                foreach (var cell in cells_pos)
                {
                    if(_i > cells_pos.Length - 1)
                        Gizmos.DrawLine(new Vector3(cell.x, 0.1f, cell.y), new Vector3(cells_pos[0].x, 0.1f, cells_pos[0].y));
                    else
                        Gizmos.DrawLine(new Vector3(cell.x, 0.1f, cell.y), new Vector3(cells_pos[_i].x, 0.1f, cells_pos[_i].y));
                    
                    _i++;
                }
            }
        }
        
        /*
        private void OnGUI()
        {
            int ix = _grid.get_allCells.GetLength(0);
            int iy = _grid.get_allCells.GetLength(1);

            float parentt = 10;
            float childd = 10 + parentt;

            for (int i = 0; i < ix; i++)
            {
                for (int j = 0; j < iy; j++)
                {
                    childd = 10 + parentt;
                    if (_grid.get_allCells[i, j] != null)
                    {
                        GUI.contentColor = Color.white;
                        GUI.Label(new Rect(10, parentt, 600, 20), "Pawn Pos : X = " + i + " Y = " + j);
                        
                        foreach (var pawn in _grid.get_allCells[i, j])
                        {
                            if (pawn != null)
                            {
                                if (pawn.GetPawnTransform().GetComponent<EnemySpawnBullet>() != null)
                                {
                                    GUI.contentColor = Color.red;
                                    GUI.Label(new Rect(50, childd, 600, 20), "Pawn Name" + pawn.GetPawnTransform().name);
                                }
                                else
                                {
                                    GUI.contentColor = Color.white;
                                    GUI.Label(new Rect(50, childd, 600, 20), "Pawn Name" + pawn.GetPawnTransform().name);
                                }
                            }
                            else
                            {
                                GUI.contentColor = Color.black;
                                GUI.Label(new Rect(50, childd, 600, 20), "null");
                            }
                            
                            childd += 15;
                        }
                    }
                    else
                    {
                        GUI.contentColor = Color.black;
                        GUI.Label(new Rect(10, parentt, 600, 20), "Not Found Pawn Pos : X = " + i + " Y = " + j);
                    }
                    
                    parentt = childd + 10;
                }
            }
        }*/
    }
}