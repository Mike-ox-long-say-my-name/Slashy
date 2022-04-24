using Core.Characters;

namespace Core.Attacking
{
    public interface IMonoHurtbox : IMonoWrapper<IHurtbox>
    {
        new IHurtbox Native { get; }
    }
}