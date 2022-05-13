namespace Core
{
    public readonly struct InteractionResult
    {
        public readonly InteractionType Type;
        public readonly object Sender;

        public InteractionResult(InteractionType type, object sender)
        {
            Type = type;
            Sender = sender;
        }
    }
}