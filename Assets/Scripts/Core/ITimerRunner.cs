using System;
using Core.Utilities;

namespace Core
{
    public interface ITimerRunner
    {
        Timer CreateTimer(Action timeout = null, bool repeating = false);
        bool RemoveTimer(Timer timer);
    }
}