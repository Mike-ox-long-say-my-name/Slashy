using Core.Attacking;

namespace Core.Characters.Interfaces
{
    public interface ICharacterEventDispatcher
    {
        void OnHealthChanged(ICharacter character, ICharacterResource health);
        void OnHitReceived(ICharacter character, HitInfo info);
        void OnHitReceivedExclusive(ICharacter character, HitInfo info);
        void OnStaggered(ICharacter character, HitInfo info);
        void OnDeath(ICharacter character, HitInfo info);
    }
}