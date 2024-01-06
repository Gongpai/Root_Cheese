using System;
using ExitGames.Client.Photon;
using GDD.Util;
using Newtonsoft.Json;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
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

        public floatMinMax impactPointArea
        {
            get => m_impactPointArea;
        }
        
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            
            //Check Is Other Will Return
            Hashtable CustomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
            object propDatas;
            if (CustomProperties != null &&
                CustomProperties.TryGetValue(PunGameSetting.PRE_RANDOMTARGETPOSITION, out propDatas))
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
        }

        private void CreatePreRandomPositionHashtable(float2D[] positions)
        {
            string json_positions = JsonConvert.SerializeObject(positions);
            Debug.Log($"Pre-Random Prop is : {json_positions}");

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

        private void OnPreRandomPositionUpdate(Hashtable propertiesChanged)
        {
            object preRandomFromProps;

            if (propertiesChanged.TryGetValue(PunGameSetting.PRE_RANDOMTARGETPOSITION, out preRandomFromProps)) {
                Debug.Log($"Update Pre-Random Prop is : {(string)preRandomFromProps}");

                float2D[] positions = JsonConvert.DeserializeObject<float2D[]>((string)preRandomFromProps);
                PunGameSetting.Pre_RandomTargetPosition = positions;
            }
        }

        private void OnRandomPositionTargetCountUpdate(Hashtable propertiesChanged)
        {
            object countFromProps;

            if (propertiesChanged.TryGetValue(PunGameSetting.RANDOMPOSITIONTARGETCOUNT, out countFromProps)) {
                Debug.Log($"Update Random Count Prop is : {(int)countFromProps}");
                PunGameSetting.RandomPositionTargetCount = (int)countFromProps;
            }
        }
    }
}