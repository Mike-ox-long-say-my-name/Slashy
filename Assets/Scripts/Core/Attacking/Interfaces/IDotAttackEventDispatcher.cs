namespace Core.Attacking.Interfaces
{
    public interface IDotAttackEventDispatcher
    {
        void OnHit(IHurtbox hit);
    }
}