namespace Core.Attacking
{
    public interface IMonoHitEventReceiver
    {
        void OnHit(IHitbox source, IHurtbox hit);
    }
}