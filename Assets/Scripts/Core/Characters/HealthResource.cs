using Core.Characters.Interfaces;

namespace Core.Characters
{
    public class HealthResource : BaseCharacterResource
    {
        public HealthResource(ICharacter character, float maxHealth) : base(character, maxHealth)
        {
        }
    }
}