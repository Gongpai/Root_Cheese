using GDD.ObjectPool;
using UnityEngine;

namespace GDD.Spawner
{
    public class SpawnerObjectsPool : ObjectPoolBuilder
    {
        [SerializeField] private GameObject m_object;
        [SerializeField] private bool m_isUseCustomPositions = false;
    }
}