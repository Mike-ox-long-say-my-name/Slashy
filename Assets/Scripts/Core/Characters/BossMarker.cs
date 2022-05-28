using Core.Characters;
using Core.Characters.Mono;

namespace Core
{
    public class BossMarker : EnemyMarker
    {
        public MixinBossEventDispatcher BossEvents => CreatedEnemy.GetComponent<MixinBossEventDispatcher>();
        public MixinCharacter Character => CreatedEnemy.GetComponent<MixinCharacter>();
    }
}