namespace Core
{
    public class Interactor
    {
        private readonly IInteractionService _interactionService;

        public Interactor(IInteractionService interactionService)
        {
            _interactionService = interactionService;
        }

        public InteractionResult TryInteract(InteractionMask mask)
        {
            return _interactionService.TryInteract(mask);
        }
    }
}