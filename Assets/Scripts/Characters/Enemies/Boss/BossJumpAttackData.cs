using JetBrains.Annotations;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    [CreateAssetMenu(menuName = "Attacks/Boss Jump Attack", fileName = "BossJumpAttackData", order = 1)]
    public class BossJumpAttackData : ScriptableObject
    {
        [field: SerializeField] public WaveAttackWalker Walker { get; [UsedImplicitly] private set; }
        [field: SerializeField] public WaveAttackSpike Spike { get; [UsedImplicitly] private set; }
        [field: SerializeField] public int Rows { get; private set; }
        [field: SerializeField] public float StepDistance { get; private set; }
        [field: SerializeField] public float StepInterval { get; private set; }
        [field: SerializeField] public float BaseOffsetDistance { get; private set; }
        [field: SerializeField] public float GroundOffset { get; private set; } = -1;
    }
}