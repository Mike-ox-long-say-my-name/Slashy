using UnityEngine;

namespace Core.Player
{
    public class MixinPlayerCapabilities : MonoBehaviour
    {
        [field: SerializeField] public bool CanJump { get; set; }
        [field: SerializeField] public bool CanDash { get; set; }
        [field: SerializeField] public bool CanLightAttack { get; set; }
        [field: SerializeField] public bool CanStrongAttack { get; set; }
        [field: SerializeField] public bool CanHeal { get; set; }
    }
}