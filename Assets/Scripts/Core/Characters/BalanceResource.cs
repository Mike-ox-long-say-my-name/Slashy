namespace Core.Characters
{
    public class BalanceResource : BaseCharacterResource
    {
        public BalanceResource(Character character, float maxValue, float startValue) : base(character, maxValue, startValue)
        {
        }

        public BalanceResource(Character character, float maxValue) : base(character, maxValue)
        {
        }
    }
}