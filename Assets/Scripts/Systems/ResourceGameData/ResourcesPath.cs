using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "ResourcePath", menuName = "GDD/ResourcePath", order = 0)]
    public class ResourcesPath : ScriptableObject
    {
        [SerializeField]
        private List<string> _paths;

        public List<string> paths
        {
            get => _paths;
            set => _paths = value;
        }
    }
}