using Core.Characters.Interfaces;

namespace Core.Attacking
{
    public struct HitSource
    {
        public bool IsEnvironmental;
        public ICharacter Character;

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