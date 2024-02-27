using System;
using System.Linq;
using ExitGames.Client.Photon;
using GDD.Util;
using Newtonsoft.Json;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace GDD.PUN
{
    public class PunRoomManager : MonoBehaviourPunCallbacks
    {
        [Header("Enemy Attack System")] 
        [SerializeField]
        private int m_randomPositionTargetCount;
        [SerializeField]
        private floatMinMax m_impactPointArea;
        private GameManager GM;
        private UnityAction<int> _selectChapterCallback;
        
        //Singleton
        protected static PunRoomManager _instance;

        public UnityAction<int> SelectChapterCallback
        {
            get => _selectChapterCallback;
            set => _selectChapterCallback = value;
        }

        public static PunRoomManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<PunRoomManager>();

                return _instance;
            }
        }
        
        public floatMinMax impactPointArea
        {
            get => m_impactPointArea;
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            CreateRoomName(PhotonNetwork.CurrentRoom.Name);
            
            GM = GameManager.Instance;
            
            //Check Is Other Will Return
            Hashtable CustomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
            if (CustomProperties != null &&
                CustomProperties.TryGetValue(PunGameSetting.PRE_RANDOMTARGETPOSITION, out object propDatas))
            {
                OnRoomInitializedForOtherClient(CustomProperties);
                return;
            }

            //Random Position Target Count
            CreateRandomPositionTargetCountHashtable(m_randomPositionTargetCount);
            
            //Pre-Random Position
            float2D[] positions = PreRandomPosition(m_randomPositionTargetCount, m_impactPointArea.min, m_impactPointArea.max);
            PunGameSetting.Pre_RandomTargetPosition = positions;
            CreatePreRandomPositionHashtable(positions);
        }

        private void OnRoomInitializedForOtherClient(Hashtable customProperties)
        {
            print($"CCC : {customProperties.Count}");
            
            //Random Position Target Count
            OnRandomPositionTargetCountUpdate(customProperties);
            
            //Pre-Random Position
            OnPreRandomPositionUpdate(customProperties);
        }

        private float2D[] PreRandomPosition(int count, float min, float max)
        {
            float2D[] positions = new float2D[count];

            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = new float2D(Random.Range(min, max), Random.Range(min, max));
            }

            return positions;
        }
        
        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            base.OnRoomPropertiesUpdate(propertiesThatChanged);

            //Random Position Target Count
            OnRandomPositionTargetCountUpdate(propertiesThatChanged);
            
            //Pre-Random Position
            OnPreRandomPositionUpdate(propertiesThatChanged);
            
            //Update Player Ready Next Level
            OnUpdateReadyNextLevelPlayer(propertiesThatChanged);
            
            //RoomNameUpdate
            OnRoomNameUpdate(propertiesThatChanged);
            
            //ChapterSelectUpdate
            OnSelectChapter(propertiesThatChanged);
        }

        private void CreatePreRandomPositionHashtable(float2D[] positions)
        {
            string json_positions = JsonConvert.SerializeObject(positions);
            //Debug.Log($"Pre-Random Prop is : {json_positions}");

            Hashtable prop = new Hashtable()
            {
                { 
                    PunGameSetting.PRE_RANDOMTARGETPOSITION, 
                    json_positions
                }
            };
                
            PhotonNetwork.CurrentRoom.SetCustomProperties(prop);
        }

        private void CreateRandomPositionTargetCountHashtable(int count)
        {
            Hashtable prop = new Hashtable()
            {
                { 
                    PunGameSetting.RANDOMPOSITIONTARGETCOUNT, 
                    count
                }
            };
                
            PhotonNetwork.CurrentRoom.SetCustomProperties(prop);
        }

        public void CreateChapterSelect(int chapter)
        {
            if (!PhotonNetwork.InRoom || !PhotonNetwork.InLobby || !PhotonNetwork.IsConnected)
                return;
            
            Hashtable prop = new Hashtable()
            {
                {
                    PunGameSetting.NUMBERCHAPTER,
                    chapter
                }
            };
            
            PhotonNetwork.CurrentRoom.SetCustomProperties(prop);
        }
        
        public void CreateUpdateReadyNextLevelPlayer()
        {
            object[] readyPlayer = new object[GM.players.Count];
            for (int i = 0; i < GM.players.Count; i++)
            {
                readyPlayer[i] = new object[]
                {
                    GM.players.Values.ElementAt(i),
                    GM.players.Keys.ElementAt(i).idPhotonView
                };
                
                //print($"Ready is VA = {GM.players.Values.ElementAt(i)}");
                //print($"Ready is Keys = {GM.players.Keys.ElementAt(i).idPhotonView}");
            }
            
            
            
            Hashtable prop = new Hashtable()
            {
                { 
                    PunGameSetting.PLAYERREADYNEXTLEVEL, 
                    readyPlayer
                }
            };
                
            PhotonNetwork.CurrentRoom.SetCustomProperties(prop);
        }
        
        private void CreateRoomName(string roomName)
        {
            Hashtable prop = new Hashtable()
            {
                { 
                    PunGameSetting.ROOMNAME, 
                    roomName
                }
            };
                
            PhotonNetwork.CurrentRoom.SetCustomProperties(prop);
        }

        private void OnPreRandomPositionUpdate(Hashtable propertiesChanged)
        {
            if (propertiesChanged.TryGetValue(PunGameSetting.PRE_RANDOMTARGETPOSITION, out object preRandomFromProps)) {
                //Debug.Log($"Update Pre-Random Prop is : {(string)preRandomFromProps}");

                float2D[] positions = JsonConvert.DeserializeObject<float2D[]>((string)preRandomFromProps);
                PunGameSetting.Pre_RandomTargetPosition = positions;
            }
        }

        private void OnRandomPositionTargetCountUpdate(Hashtable propertiesChanged)
        {
            if (propertiesChanged.TryGetValue(PunGameSetting.RANDOMPOSITIONTARGETCOUNT, out object countFromProps)) {
                //Debug.Log($"Update Random Count Prop is : {(int)countFromProps}");
                PunGameSetting.RandomPositionTargetCount = (int)countFromProps;
            }
        }

        private void OnSelectChapter(Hashtable propertiesChanged)
        {
            if (propertiesChanged.TryGetValue(PunGameSetting.NUMBERCHAPTER, out object chapter))
            {
                int chapterNumber = (int)chapter;
                GM.selectChapter = chapterNumber;
                print($"Chapter is : {chapterNumber}");
                _selectChapterCallback?.Invoke(chapterNumber);
            }
        }

        private void OnUpdateReadyNextLevelPlayer(Hashtable propertiesChanged)
        {
            if (propertiesChanged.TryGetValue(PunGameSetting.PLAYERREADYNEXTLEVEL, out object playerReadyStates)) 
            {
                for (int i = 0; i < GM.players.Count; i++)
                {
                    if (GM.players.Keys.ElementAt(i).isMasterClient)
                    {
                        object[] data = (object[])playerReadyStates;
                        PhotonView photonView = PhotonNetwork.GetPhotonView((int)((object[])data[i])[1]);
                        //print($"{photonView.gameObject.name} Ready is [F] = {(int)((object[])data[i])[1]} || [R] = {(bool)((object[])data[i])[0]}");
                        PlayerSystem player = photonView.gameObject.GetComponent<PlayerSystem>();
                        GM.players[player] = (bool)((object[])data[i])[0];
                        player.UpdateReadyCheckUI();
                    }
                }
            }
        }

        private void OnRoomNameUpdate(Hashtable propertiesChanged)
        {
            if (propertiesChanged.TryGetValue(PunGameSetting.ROOMNAME, out object nameFromProps))
            {
                //Debug.Log($"Update Random Count Prop is : {(int)countFromProps}");
                PunGameSetting.roomName = (string)nameFromProps;
            }
        }
    }
}