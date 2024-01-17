using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "NewCharacterStatusPicker", menuName = "GDD/CharacterStatusPicker", order = 1)]
    public class CharacterStatusPicker : ScriptableObject
    {
        [SerializeField][SerializedDictionary("Name", "Amount")]
        private SerializedDictionary<string, SerializedDictionary<float, float>> m_Picker = new SerializedDictionary<string, SerializedDictionary<float, float>>();

        public List<Tuple<string, float, float>> picker
        {
            get
            {
                return m_Picker.Select(x =>
                        new Tuple<string, float, float>(
                            x.Key, 
                            x.Value.Keys.ElementAt(0), 
                            x.Value.Values.ElementAt(0)
                        )).ToList();
            }
        }
    }
}