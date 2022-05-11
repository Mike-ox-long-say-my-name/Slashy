using UnityEngine;

namespace Core.Player
{
    [CreateAssetMenu(menuName = "Other/Player Caps", fileName = "PlayerCapabilities", order = 2)]
    public class PlayerCapabilitiesSO : ScriptableObject
    {
        [field: SerializeField] public bool CanJump { get; set; }
        [field: SerializeField] public bool CanDash { get; set; }
        [field: SerializeField] public bool CanLightAttack { get; set; }
        [field: SerializeField] public bool CanStrongAttack { get; set; }
        [field: SerializeField] public bool CanHeal { get; set; }
    }
}