using UnityEngine;

namespace GDD
{
    public interface IAiState
    {
        public Vector3 EnterState();

        public void UpdateState();

        public void ExitState();
    }
}