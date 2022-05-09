using Core.Characters.Interfaces;
using UnityEngine;

namespace Core.Attacking
{
    public struct HitSource
    {
        public Transform Transform { get; set; }
        public bool IsEnvironmental { get; set; }
        public ICharacter Character { get; set; }

        public static HitSource Environmental => new HitSource
        {
            IsEnvironmental = true,
            Character = null
        };

        public static HitSource None => new HitSource
        {
            IsEnvironmental = false,
            Character = null
        };

        public static HitSource AsCharacter(ICharacter character)
        {
            return new HitSource
            {
                Character = character,
                IsEnvironmental = false
            };
        }
    }
}