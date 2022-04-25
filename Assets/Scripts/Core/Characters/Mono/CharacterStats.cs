using Core.Characters.Interfaces;
using UnityEngine;

namespace Core.Characters.Mono
{
    [CreateAssetMenu(menuName = "Character Stats", fileName = "CharacterStats", order = 0)]
    public class CharacterStats : ScriptableObject, ICharacterStats
    {
        [SerializeField, Min(0)] private float maxHealth;
        [SerializeField, Min(0)] private float maxBalance;
        [SerializeField] private bool freezeHealth = false;
        [SerializeField] private bool canDie = true;

        public float MaxHealth => maxHealth;
        public float MaxBalance => maxBalance;
        public bool FreezeHealth => freezeHealth;
        public bool CanDie => canDie;
    }
}