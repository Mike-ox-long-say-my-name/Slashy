namespace Core
{
    public interface IInteractable
    {
        bool IsInteractable { get; }
        InteractionMask Mask { get; }
        InteractionResult Interact();
    }
}