using UnityEngine;

namespace Core
{
    public class MixinInteractor : MonoBehaviour
    {
        public InteractionResult TryInteract(InteractionMask mask)
        {
            return InteractionManager.Instance.TryInteract(mask);
        }
    }
}