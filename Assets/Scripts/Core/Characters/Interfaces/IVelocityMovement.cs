using Core.Modules;
using UnityEngine;

namespace Core.Characters.Interfaces
{
    public interface IVelocityMovement : IUpdateable
    {
        float HorizontalSpeed { get; set; }
        float VerticalSpeed { get; set; }
        float MinVelocity { get; set; }
        float MaxVelocity { get; set; }
        float Gravity { get; set; }
        float GroundedGravity { get; set; }
        float AirboneControlFactor { get; set; }

        Vector3 Velocity { get; set; }
        
        void Move(Vector3 direction);
    }
}