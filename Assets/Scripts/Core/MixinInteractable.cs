using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class MixinInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool initiallyInteractable;
        [SerializeField] private InteractionType returnType;
        [SerializeField] private InteractionMask mask;
        [SerializeField] private UnityEvent interacted;

        public UnityEvent Interacted => interacted;

        private void Awake()
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
            Interacted?.Invoke();
            return new InteractionResult(returnType, this);
        }
    }
}