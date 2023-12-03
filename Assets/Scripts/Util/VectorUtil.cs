using UnityEngine;

namespace GDD.Util
{
    public static class VectorUtil
    {
        public static Vector3 GetVectorDistance(Vector3 a, Vector3 b, float distance)
        {
            Vector3 result = a - b;
            result = Vector3.Normalize(result);
            result *= distance;
            result += b;
            return result;
        }
    }
}