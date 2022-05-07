using Core.Modules;
using UnityEngine;

namespace Core.Characters.Interfaces
{
    public interface IPushable : IUpdateable
    {
        bool IsPushing { get; }

        void Push(Vector3 direction, float force);
        void Push(Vector3 direction, float force, float pushTime);
    }
}