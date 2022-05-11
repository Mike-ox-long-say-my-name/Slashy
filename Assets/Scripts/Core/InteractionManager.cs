using System.Linq;
using UnityEngine;

namespace Core
{
    [DefaultExecutionOrder(-50)]
    public class InteractionManager : PublicSingleton<InteractionManager>
    {
        protected override void Awake()
        {
            base.Awake();

            _interactables = FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>().ToArray();
        }

        private IInteractable[] _interactables;

        public InteractionResult TryInteract(InteractionMask mask)
        {
            return _interactables
                .Where(interactable => interactable.IsInteractable && ((interactable.Mask & mask) != 0))
                .Select(interactable => interactable.Interact())
                .FirstOrDefault();
        }

        private void OnDestroy()
        {
            Singleton<InteractionManager>.Instance = null;
        }
    }
}