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
using Newtonsoft.Json;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Grid = GDD.Spatial_Partition.Grid;

namespace GDD
{
    public class GameManager : DontDestroy_Singleton<GameManager>
    {
        //GameInstance
        [SerializeField] private GameInstance _GI = new GameInstance();
        private PlayerInfo _playerInfo = new PlayerInfo();
        
        //Pause Menu
        [SerializeField] private GameObject m_pauseMenu;
        
        //GameOver Menu
        [SerializeField] private GameObject m_gameOverMenu;
        
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
        [SerializeField] 
        private GameObject m_warningUI;

        [Header("Play Mode")] 
        [SerializeField] 
        private PlayMode m_playMode;

        [Header("OpenLevel")] 
        [SerializeField] private string m_mainMenuScene = "MainMenuScene";
        [SerializeField] private string m_openLevelName;
        [SerializeField] private bool m_isUnLoadSceneReSetGameInstance;
        
        private GameObject _bullet_pool;
        private Grid _grid;
        private AwaitTimer readyTimer;
        private DataBaseController DBC;
        private Canvas_Element_List _warningUI;
        private GameObject pauseMenu;
        private GameObject gameOverMenu;
        private GameState _currentGameState = GameState.Start;
        private float openSceneTime = 0;
        private int m_selectChapter;

        public GameInstance gameInstance
        {
            get => _GI;
            set => _GI = value;
        }

        public string openLevelName
        {
            get => m_openLevelName;
            set => m_openLevelName = value;
        }

        public bool isUnLoadSceneReSetGameInstance
        {
            get => m_isUnLoadSceneReSetGameInstance;
            set => m_isUnLoadSceneReSetGameInstance = value;
        }

        public GameObject pauseMenuUI
        {
            set => m_pauseMenu = value;
        }

        public GameState gameState
        {
            get => _currentGameState;
        }

        public GameState _gameState
        {
            get => _currentGameState;
            set
            {
                PunNetworkManager PNM = PunNetworkManager.Instance;
                if (PNM != null && value != _currentGameState)
                {
                    switch (value)
                    {
                        case GameState.Playing:
                            PNM.currentGameState = PunGameState.GamePlay;
                            break;
                        case GameState.Win:
                            PNM.currentGameState = PunGameState.GameWin;
                            break;
                        case GameState.GameOver:
                            PNM.currentGameState = PunGameState.GameOver;
                            break;
                        case GameState.Start:
                            PNM.currentGameState = PunGameState.GameStart;
                            break;
                        default:
                            break;
                    }
                }

                _currentGameState = value;
            }
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

        public int selectChapter
        {
            get => m_selectChapter;
            set => m_selectChapter = value;
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
            ResetGird();
            SceneManager.sceneLoaded += LoadScene;
        }

        // Start is called before the first frame update
        void Start()
        {
            NewTimerReady();
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
                CreateOrOpenPauseMenu();
            
            UpdateTimeWarningUI(openSceneTime);
            
            if(_gameState != GameState.Start )
                _gameState = m_enemies.Count > 0 ? GameState.Playing : GameState.Win;
            else if (m_enemies.Count > 0)
                _gameState = GameState.Playing;

            PlayerAllDown();
        }

        public void GetMultiPlayerGameSate(PunGameState currentGameState)
        {
            switch (currentGameState)
            {
                case PunGameState.GameOver:
                    _currentGameState = GameState.GameOver;
                    break;
                case PunGameState.GamePlay:
                    _currentGameState = GameState.Playing;
                    break;
                case PunGameState.GameStart:
                    _currentGameState = GameState.Start;
                    break;
                case PunGameState.GameWin:
                    _currentGameState = GameState.Win;
                    break;
            }
        }

        public void NewTimerReady()
        {
            readyTimer = new AwaitTimer(5.0f, () =>
            {
                //print("Update SaveGame To Server");
                HideWarningUI();
                //Update Save
            
                if (m_isUnLoadSceneReSetGameInstance)
                {
                    print("Send Reset");
                    ResetGameInstance();
                }
                
                DBC.OnUpdateSucceed += UpdateSaveGameServer;
                DBC.OnUpdate(playerInfo, gameInstance);
                
                NewTimerReady();
            }, time =>
            {
                openSceneTime = time;
                //print($"Time : {time}");
            });
        }

        public void PlayerAllDown()
        {
            bool isAllPlayerDown = false;
            foreach (var player in m_players)
            {
                isAllPlayerDown = player.Key.GetHP() <= 0;
                
                if(!isAllPlayerDown)
                    break;
            }

            if (isAllPlayerDown)
            {
                _gameState = GameState.GameOver;
                CreateOrOpenGameOverMenu();
            }
        }
        
        public void CreateOrOpenPauseMenu()
        {
            if(m_pauseMenu == null)
                return;
            
            if (pauseMenu == null)
            {
                pauseMenu = Instantiate(m_pauseMenu, Vector3.zero, quaternion.identity);
            }
            else
            {
                pauseMenu.SetActive(true);
            }
            
            GameStateManager.Instance.SetState(GameState.Pause);
        }

        public void CreateOrOpenGameOverMenu()
        {
            if (gameOverMenu == null)
                gameOverMenu = Instantiate(m_gameOverMenu, Vector3.zero, quaternion.identity);
            else
                gameOverMenu.SetActive(true);
            
            GameStateManager.Instance.SetState(GameState.Pause);
        }

        public void ResetGird()
        {
            _grid = new Grid((int)m_mapWidth, m_cellSize, enemies.Count);
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
                //print("Ready Check !!!");
                readyTimer.Start();
                Canvas_Element_List canvasElementList = CreateWarningUI();
                string[] levelNames = PunLevelManager.Instance.openLevel.Split("-");
                string levelName;
                levelName = levelNames.Length > 1 ? levelNames[1] : levelNames[0];
                canvasElementList.texts[1].text = $"Entering to {levelName}";
            }
            else
            {
                HideWarningUI();
                readyTimer.Stop();
            }
        }

        private Canvas_Element_List CreateWarningUI()
        {
            if (_warningUI == null)
            {
                _warningUI = Instantiate(m_warningUI).GetComponent<Canvas_Element_List>();
                _warningUI.transform.position = Vector3.zero;
                _warningUI.animators[1].enabled = true;
                _warningUI.animators[0].SetBool("IsPlay", true);

                return _warningUI;
            }
            else
            {
                _warningUI.animators[1].enabled = true;
                _warningUI.animators[0].SetBool("IsPlay", true);
                
                return _warningUI;
            }
        }

        public void UpdateTimeWarningUI(float time)
        {
            if(_warningUI == null || !_warningUI.gameObject.activeSelf)
                return;
            
            float showTime = 5.0f - time;
            _warningUI.images[0].fillAmount = showTime / 5.0f;
            _warningUI.texts[0].text = ((int)showTime).ToString();
        }

        private void HideWarningUI()
        {
            if(_warningUI == null)
                return;
            
            print("Wait Other Player");
            Canvas_Element_List canvasElementList = _warningUI.GetComponent<Canvas_Element_List>();
            canvasElementList.animators[1].enabled = false;
            canvasElementList.animators[0].SetBool("IsPlay", false);
        }

        public void ResetGameInstance()
        {
            print("Reset GameInstance");
            _GI = new GameInstance();
        }
        
        private async void UpdateSaveGameServer()
        {
            print("Sync Save Game");
            
            if(PhotonNetwork.IsMasterClient)
                DBC.OnSyncSucceed += PunLoadLevel;
            await DBC.OnSync();
            DBC.OnUpdateSucceed -= UpdateSaveGameServer;
            
            print($"GameInstance : {JsonConvert.SerializeObject(DBC.dataBase.data.gameSave)}");
        }
        
        public void PunLoadLevel()
        {
            //Pun System
            print("Next Level");
            SceneManager.sceneUnloaded += UnloadScene;
            PhotonNetwork.LoadLevel(m_openLevelName);
            PunNetworkManager.Instance.isLoadLevel = true;
            
            DBC.OnSyncSucceed -= PunLoadLevel;
        }
        
        public async Task PunLoadLevel(string scene)
        {
            //Pun System
            print("Next Level");
            ResetGameInstance();
            await DBC.OnUpdate(playerInfo, gameInstance);
            await DBC.OnSync();
            
            SceneManager.sceneUnloaded += UnloadScene;
            PhotonNetwork.LoadLevel(scene);
            PunNetworkManager.Instance.isLoadLevel = true;
        }

        private void LoadScene(Scene scene, LoadSceneMode mode)
        {
            PunNetworkManager PNM = PunNetworkManager.Instance;
            
            if(PNM != null)
                PNM.onGameStateUpdate += GetMultiPlayerGameSate;
        }
        
        private void UnloadScene(Scene scene)
        { 
            ResetGird();
            _gameState = GameState.Start;
            
            PunNetworkManager PNM = PunNetworkManager.Instance;
            if(PNM != null)
                PNM.onGameStateUpdate -= GetMultiPlayerGameSate;
            
            SceneManager.sceneUnloaded -= UnloadScene;
            
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

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= LoadScene;
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