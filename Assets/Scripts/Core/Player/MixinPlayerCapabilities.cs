using UnityEngine;

namespace Core.Player
{
    public class MixinPlayerCapabilities : MonoBehaviour
    {
        [SerializeField] private bool modifySO = false;
        [SerializeField] private PlayerCapabilitiesSO playerCapabilities;

        public bool CanJump
        {
            get => playerCapabilities.CanJump;
            set
            {
                if (modifySO)
                {
                    playerCapabilities.CanJump = value;
                }
            }
        }
        public bool CanDash
        {
            get => playerCapabilities.CanDash;
            set
            {
                if (modifySO)
                {
                    playerCapabilities.CanDash = value;
                }
            }
        }
        public bool CanLightAttack
        {
            get => playerCapabilities.CanLightAttack;
            set
            {
                if (modifySO)
                {
                    playerCapabilities.CanLightAttack = value;
                }
            }
        }
        public bool CanStrongAttack
        {
            get => playerCapabilities.CanStrongAttack;
            set
            {
                if (modifySO)
                {
                    playerCapabilities.CanStrongAttack = value;
                }
            }
        }
        public bool CanHeal
        {
            get => playerCapabilities.CanHeal;
            set
            {
                if (modifySO)
                {
                    playerCapabilities.CanHeal = value;
                }
            }
        }
    }
}