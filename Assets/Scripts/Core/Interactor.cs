using System;
using UnityEngine;

namespace Core
{
    public class Interactor
    {
        private readonly InteractionService _interactionService;

        public Interactor(InteractionService interactionService)
        {
            _interactionService = interactionService;
        }

        public InteractionResult TryInteract(InteractionMask mask)
        {
            return _interactionService.TryInteract(mask);
        }
    }
}