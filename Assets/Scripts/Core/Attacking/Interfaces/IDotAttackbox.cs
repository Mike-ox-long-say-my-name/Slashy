namespace Core.Attacking.Interfaces
{
    public interface IDotAttackbox : IHitbox
    {
        void ProcessHit(IHurtbox hit);
        void Tick(float deltaTime);
    }
}