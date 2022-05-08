using Core.Characters.Interfaces;
using Core.Modules;
using UnityEngine;

namespace Core.Characters
{
    public abstract class StaminaRecoveryPolicy : ScriptableObject, IUpdateable
    {
        public abstract float RecoverySpeed { get; }
        public abstract bool CanRecover { get; }

        public abstract void InformSpent(IResource stamina);
        public abstract void Tick(float deltaTime);
    }
}