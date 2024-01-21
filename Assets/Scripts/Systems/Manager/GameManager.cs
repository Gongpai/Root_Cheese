using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AYellowpaper.SerializedCollections;
using GDD.DataBase;
using GDD.JsonHelper;
using GDD.PUN;
using GDD.Sinagleton;
using GDD.Timer;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Grid = GDD.Spatial_Partition.Grid;

namespace GDD
{
    public class GameManager : DontDestroy_Singleton<GameManager>
    {
        //GameInstance
        [SerializeField] private GameInstance _GI = new GameInstance();
        private PlayerInfo _playerInfo = new PlayerInfo();
        
        //Game Setting
        [Header("Finding System")] 
        [SerializeField]
        private SerializedDictionary<PlayerSystem, bool> m_players = new SerializedDictionary<PlayerSystem, bool>();
        
        [SerializeField]
        private Transform m_player_layer;
        
        [SerializeField]
        private List<EnemySystem> m_enemies = new List<EnemySystem>();
        
        [SerializeField]
        private Transform m_enemy_layer;
        
        [SerializeField]
        private float m_mapWidth = 50f;
        
        [SerializeField]
        private int m_cellSize = 10;

        [Header("Player Client")] 
        [SerializeField]
        private PlayerSystem m_playerMasterClient;

        [Header("Play Mode")] 
        [SerializeField] 
        private PlayMode m_playMode;
        
        private GameObject _bullet_pool;
        private Grid _grid;
        private AwaitTimer readyTimer;
        private DataBaseController DBC;

        public GameInstance gameInstance
        {
            get => _GI;
            set => _GI = value;
        }

        public PlayerInfo playerInfo
        {
            get => _playerInfo;
            set => _playerInfo = value;
        }
        
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
        
        public SerializedDictionary<PlayerSystem, bool> players
        {
            get => m_players;
            set => m_players = value;
        }

        public PlayerSystem playerMasterClient
        {
            get => m_playerMasterClient;
            set => m_playerMasterClient = value;
        }

        public Transform player_layer
        {
            get => m_player_layer;
            set => m_player_layer = value;
        }

        public List<EnemySystem> enemies
        {
            get => m_enemies;
            set => m_enemies = value;
        }
        
        public Transform enemy_layer
        {
            get => m_enemy_layer;
            set => m_enemy_layer = value;
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
            DBC = DataBaseController.Instance;
        }

        private void OnEnable()
        {
            _grid = new Grid((int)m_mapWidth, m_cellSize, enemies.Count);
        }

        // Start is called before the first frame update
        void Start()
        {
            readyTimer = new AwaitTimer(5.0f, () =>
            {
                print("Update SaveGame To Server");
                //Update Save
                DBC.OnUpdate(playerInfo, gameInstance);
                DBC.OnUpdateSucceed += UpdateSaveGameServer;
            }, time =>
            {
                //print($"Time is = {time}");
            });
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        
        public void OnReady(bool isLobby = false)
        {
            bool readyPlayer = false;
            
            foreach (var player in players)
            {
                if (!player.Value)
                {
                    readyPlayer = false;
                    break;
                }

                readyPlayer = player.Key;
            }

            if (readyPlayer && (enemies.Count <= 0 || isLobby))
            {
                readyTimer.Start();
            }
            else
            {
                print("Wait Other Player");
                readyTimer.Stop();
            }
        }

        private void UpdateSaveGameServer()
        {
            print("Sync Save Game");
            DBC.OnSync();
            DBC.OnSyncSucceed += PunLoadLevel;
            DBC.OnUpdateSucceed -= UpdateSaveGameServer;
        }
        
        private void PunLoadLevel()
        {
            //Pun System
            print("Next Level");
            
            if (PhotonNetwork.InRoom && !PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LeaveRoom();
            }
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            SceneManager.LoadSceneAsync(PunLevelManager.Instance.openLevel);
            PunNetworkManager.Instance.isLoadLevel = true;
            print($"GameInstance : {JsonHelperScript.CreateJsonObject<GameInstance>(gameInstance)}");
            
            DBC.OnSyncSucceed -= PunLoadLevel;
        }
        
        void OnDrawGizmosSelected()
        {
            foreach (var _player in players)
            {
                Vector2 p_vision = _player.Key.GetComponent<PlayerSystem>().Get_Vision / 2;
                //Determine which grid cell the friendly soldier is in
                float cellX = _player.Key.GetPawnTransform().position.x;
                float cellZ = _player.Key.GetPawnTransform().position.z;
                Vector2[] cells_pos = new Vector2[4];
                cells_pos[0] = new Vector2(cellX + p_vision.x, cellZ - p_vision.y);
                cells_pos[1] = new Vector2(cellX + p_vision.x, cellZ + p_vision.y);
                cells_pos[2] = new Vector2(cellX - p_vision.x, cellZ + p_vision.y);
                cells_pos[3] = new Vector2(cellX - p_vision.x, cellZ - p_vision.y);
                
                Gizmos.color = new Color(0, 1, 0, 0.5f);
                Gizmos.DrawCube(_player.Key.GetPawnTransform().position, new Vector3(_player.Key.Get_Vision.x, 1, _player.Key.Get_Vision.y));
                
                
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