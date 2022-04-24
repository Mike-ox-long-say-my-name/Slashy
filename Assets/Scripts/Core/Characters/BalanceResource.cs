namespace Core.Characters
{
    public class BalanceResource : BaseCharacterResource
    {
        public BalanceResource(ICharacter character, float maxValue, float startValue) : base(character, maxValue, startValue)
        {
        }

        public BalanceResource(ICharacter character, float maxValue) : base(character, maxValue)
        {
        }
    }
}