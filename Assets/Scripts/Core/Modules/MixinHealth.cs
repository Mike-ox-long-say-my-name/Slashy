using Core.Characters;
using Core.Characters.Interfaces;
using Core.Characters.Mono;
using UnityEngine;

namespace Core.Modules
{
    public class MixinHealth : MixinResource
    {
        private IResource _health;

        public IResource Health
        {
            get
            {
                if (_health != null)
                {
                    return _health;
                }

                _health = new HealthResource(MaxValue);
                return _health;
            }
        }

        public override IResource Resource => Health;
    }
}