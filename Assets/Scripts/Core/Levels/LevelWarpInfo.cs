using UnityEngine;

namespace Core.Levels
{
    public struct LevelWarpInfo
    {
        public readonly string LevelName;
        public readonly Vector3 StartPosition;
        public readonly Vector3 StartMoveTarget;

        public LevelWarpInfo(string levelName, Vector3 startPosition, Vector3 startMoveTarget)
        {
            LevelName = levelName;
            StartPosition = startPosition;
            StartMoveTarget = startMoveTarget;
        }
    }
}