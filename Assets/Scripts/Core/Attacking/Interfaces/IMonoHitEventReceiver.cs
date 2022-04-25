namespace Core.Attacking.Interfaces
{
    public interface IMonoHitEventReceiver
    {
        void OnHit(IHitbox source, IHurtbox hit);
    }
}