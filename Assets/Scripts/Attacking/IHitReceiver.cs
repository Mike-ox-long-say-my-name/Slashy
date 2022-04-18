namespace Attacking
{
    public interface IHitReceiver
    {
        void ReceiveHit(in HitInfo info);
    }
}