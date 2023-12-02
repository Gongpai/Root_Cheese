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
        }
    }
}