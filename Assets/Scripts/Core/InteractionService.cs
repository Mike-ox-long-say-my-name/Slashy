using System.Linq;
using UnityEngine;

namespace Core
{
    public class InteractionService : IInteractionService
    {
        private readonly IInteractable[] _interactables;

        public InteractionService(IObjectLocator objectLocator)
        {
            _interactables = objectLocator.FindAll<IInteractable>();
        }

        public InteractionResult TryInteract(InteractionMask mask)
        {
            return _interactables
                .Where(interactable => interactable.IsInteractable && ((interactable.Mask & mask) != 0))
                .OrderBy(interactable => interactable.Mask)
                .Select(interactable => interactable.Interact())
                .FirstOrDefault();
        }
    }
}