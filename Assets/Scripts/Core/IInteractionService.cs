namespace Core
{
    public interface IInteractionService
    {
        InteractionResult TryInteract(InteractionMask mask);
    }
}