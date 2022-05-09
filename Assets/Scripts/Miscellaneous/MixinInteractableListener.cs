using System;
using Core;
using Core.Player;
using Settings;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Miscellaneous
{
    [RequireComponent(typeof(MixinInteractable))]
    public class MixinInteractableListener : MonoBehaviour
    {
        private MixinInteractable _interactable;
        private Transform _interactor;

        private void Awake()
        {
            _interactable = GetComponent<MixinInteractable>();
            _interactor = PlayerManager.Instance.PlayerInfo.Transform;
        }

        private void OnEnable()
        {
            SubscribeToInteract();
        }

        private void OnDisable()
        {
            UnsubscribeFromInteract();
        }

        private void SubscribeToInteract()
        {
            var actions = PlayerActionsProxy.Instance.Actions;
            actions.Player.Interact.performed += RaiseInteraction;
        }

        private void UnsubscribeFromInteract()
        {
            var actions = PlayerActionsProxy.Instance.Actions;
            actions.Player.Interact.performed -= RaiseInteraction;
        }

        private void RaiseInteraction(InputAction.CallbackContext obj)
        {
            _interactable.TryInteract(_interactor);
        }
    }
}