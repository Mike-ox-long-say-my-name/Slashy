using System;

namespace Core
{
    [Flags]
    public enum InteractionMask
    {
        Any = int.MaxValue,
        Popup = 1 << 0,
        Bonfire = 1 << 1
    }
}