using Core.Attacking;

namespace Attacks
{
    public interface IDotAttackEventDispatcher
    {
        void OnHit(IHurtbox hit);
    }
}