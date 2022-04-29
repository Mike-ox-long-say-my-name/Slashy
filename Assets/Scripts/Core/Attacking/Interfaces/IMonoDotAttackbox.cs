using Core.DependencyInjection;

namespace Core.Attacking.Interfaces
{
    public interface IMonoDotAttackbox
    {
        IDotAttackbox Attackbox { get; }
    }
}