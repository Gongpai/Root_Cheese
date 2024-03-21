using System.Collections.Generic;
using GDD.Sinagleton;
using UnityEngine;

namespace Test
{
    public class TestScript2 : CanDestroy_Sinagleton<TestScript2>
    {
        [SerializeField] public List<Transform> m_players;
    }
}