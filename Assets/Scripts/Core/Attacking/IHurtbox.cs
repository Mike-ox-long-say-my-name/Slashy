namespace Core.Attacking
{
    public interface IHurtbox : IHitbox
    {
        IHitReceiver AttachedTo { get; }

        void Dispatch(HitInfo info);
    }
}