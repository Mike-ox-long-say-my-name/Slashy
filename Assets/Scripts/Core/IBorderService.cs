using UnityEngine;

namespace Core
{
    public interface IBorderService
    {
        float GetAvailableCameraX(Camera cam, float targetX, float minDistance);
    }
}