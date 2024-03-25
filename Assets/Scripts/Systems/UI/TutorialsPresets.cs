using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GDD
{
    [CreateAssetMenu(fileName = "NewTutorialsPresets", menuName = "GDD/Tutorials", order = 0)]
    public class TutorialsPresets : ScriptableObject
    {
        [SerializeField][TextArea] private string m_text;
        [SerializeField] private Sprite m_tutorials;
        [FormerlySerializedAs("m_button")] [SerializeField] private List<Sprite> m_buttons;

        public string text
        {
            get => m_text;
        }
        public Sprite tutorials
        {
            get => m_tutorials;
        }
        public List<Sprite> buttons{
            
            get => m_buttons;
        }
    }  
}