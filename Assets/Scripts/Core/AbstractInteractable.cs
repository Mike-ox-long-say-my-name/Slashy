using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public abstract class AbstractInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool initiallyInteractable;
        [SerializeField] private InteractionType returnType;
        [SerializeField] private InteractionMask mask;

        private void OnValidate()
        {
            IsInteractable = initiallyInteractable;
        }

        public void MakeInteractable()
        {
            IsInteractable = true;
        }

        public void MakeUninteractable()
        {
            IsInteractable = false;
        }

        public bool IsInteractable { get; private set; }
        public InteractionMask Mask => mask;

        public InteractionResult Interact()
        {
            var sender = InteractInternal();
            return new InteractionResult(returnType, sender);
        }

        protected abstract object InteractInternal();
    }
}