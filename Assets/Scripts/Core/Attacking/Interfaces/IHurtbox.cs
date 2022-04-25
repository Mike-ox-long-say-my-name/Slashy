namespace Core.Attacking.Interfaces
{
    public interface IHurtbox : IHitbox
    {
        IHitReceiver AttachedTo { get; }

        void Dispatch(HitInfo info);
    }
}