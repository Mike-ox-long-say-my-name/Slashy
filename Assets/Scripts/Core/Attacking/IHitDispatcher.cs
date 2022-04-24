namespace Core.Attacking
{
    public interface IHitDispatcher
    {
        void OnHit(IHitbox source, IHurtbox target);
    }
}