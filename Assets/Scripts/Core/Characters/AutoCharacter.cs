namespace Core.Characters
{
    public class AutoCharacter : Character
    {
        public IAutoMovement AutoMovement => VelocityMovement as IAutoMovement;

        public AutoCharacter(IAutoMovement movement, DamageStats damageStats, CharacterStats characterStats)
            : base(movement, damageStats, characterStats)
        {
        }
    }
}