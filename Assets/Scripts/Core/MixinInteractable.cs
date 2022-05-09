using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class MixinInteractable : MonoBehaviour
    {
        [SerializeField] private bool hasInteractRadius = true;
        [SerializeField, Min(0)] private float interactRadius = 1f;

        [SerializeField] private UnityEvent interacted;

        public UnityEvent Interacted => interacted;

        public void TryInteract(Transform interactor)
        {
            if (hasInteractRadius)
            {
                var distance = Vector3.Distance(interactor.position, transform.position);
                if (distance > interactRadius)
                {
                    return;
                }
            }

            Interacted?.Invoke();
        }
    }
}