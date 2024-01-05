using System;
using ExitGames.Client.Photon;
using GDD.Util;
using Newtonsoft.Json;
using Photon.Pun;
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

            Hashtable CustomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
            object propDatas;
            if(CustomProperties != null && CustomProperties.TryGetValue(PunGameSetting.PRE_RANDOMTARGETPOSITION, out propDatas))
                return;
            
            float[] positions = PreRandomPosition(m_randomPositionTargetCount, m_impactPointArea.min, m_impactPointArea.max);
            CreatePreRandomPositionHashtable(positions);
        }

        private float[] PreRandomPosition(int count, float min, float max)
        {
            float[] positions = new float[count];

            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = Random.Range(min, max);
            }

            return positions;
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            base.OnRoomPropertiesUpdate(propertiesThatChanged);
            print($"CCC : {propertiesThatChanged.Count}");
            OnPreRandomPositionUpdate(propertiesThatChanged);
        }

        private void CreatePreRandomPositionHashtable(float[] positions)
        {
            string json_positions = JsonConvert.SerializeObject(positions);

            Hashtable prop = new Hashtable()
            {
                { 
                    PunGameSetting.PRE_RANDOMTARGETPOSITION, 
                    json_positions
                }
            };
                
            PhotonNetwork.CurrentRoom.SetCustomProperties(prop);
            
            /*
            if (PhotonNetwork.CurrentRoom.CustomProperties == null)
            {
                
            }
            else
                PhotonNetwork.CurrentRoom.CustomProperties.Add(PunGameSetting.PRE_RANDOMTARGETPOSITION, json_positions);
                */
        }

        private void OnPreRandomPositionUpdate(Hashtable propertiesChanged)
        {
            object preRandomFromProps;

            if (propertiesChanged.TryGetValue(PunGameSetting.PRE_RANDOMTARGETPOSITION, out preRandomFromProps)) {
                Debug.Log($"GetStartTime Prop is : {(string)preRandomFromProps}");

                float[] positions = JsonConvert.DeserializeObject<float[]>((string)preRandomFromProps);
                PunGameSetting.Pre_RandomTargetPosition = positions;
            }
        }
    }
}