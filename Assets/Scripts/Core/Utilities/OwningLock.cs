namespace Core.Utilities
{
    public class PersistentLock
    {
        private object _owner;
        
        public bool IsLocked { get; private set; }
        public bool IsUnlocked => !IsLocked;

        public void Lock(object owner = null)
        {
            IsLocked = true;
            _owner ??= owner;
        }

        public bool TryUnlock(object owner = null)
        {
            if (HasOwner && !IsOwner(owner))
            {
                return false;
            }

            _owner = null;
            IsLocked = false;
            return true;
        }

        public void ReleaseOwnership(object owner)
        {
            if (IsOwner(owner))
            {
                _owner = null;
            }
        }

        public bool HasOwner => _owner != null;

        public bool IsOwner(object owner)
        {
            return ReferenceEquals(owner, _owner);
        }

        public static implicit operator bool(PersistentLock persistentLock)
        {
            return persistentLock.IsUnlocked;
        }
    }
}