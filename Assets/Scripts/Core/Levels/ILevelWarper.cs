using UnityEngine;

namespace Core.Levels
{
    public interface ILevelWarper
    {
        bool CanInitiateWarp { get; }   
        bool IsWarping { get; }
        PlayerCreationInfo CreationInfo { get; }
        void InitiateWarp(Vector3 position, LevelWarpInfo info);
        void ConfirmWarpEnd();
    }
}