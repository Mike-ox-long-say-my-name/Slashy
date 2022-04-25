namespace Core.Attacking.Interfaces
{
    public interface IAttackbox : IHitbox
    {
        void ProcessHit(IHurtbox hit);
        void ClearHits();
    }
}