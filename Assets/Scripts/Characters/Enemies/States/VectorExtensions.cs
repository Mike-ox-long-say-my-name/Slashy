using UnityEngine;

namespace Characters.Enemies.States
{
    public static class VectorExtensions
    {
        public static Vector3 WithZeroY(this Vector3 vector)
        {
            vector.y = 0;
            return vector;
        }
    }
}