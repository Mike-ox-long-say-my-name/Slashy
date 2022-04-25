namespace Core.Attacking.Interfaces
{
    public interface IHitDispatcher
    {
        void OnHit(IHitbox source, IHurtbox target);
    }
}