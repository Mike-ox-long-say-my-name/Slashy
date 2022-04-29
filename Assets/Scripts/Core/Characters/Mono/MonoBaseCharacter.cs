using Core.Characters.Interfaces;
using UnityEngine;

namespace Core.Characters.Mono
{
    public abstract class MonoBaseCharacter : MonoBehaviour
    {
        private ICharacter _character;

        public ICharacter Character
        {
            get
            {
                if (_character != null)
                {
                    return _character;
                }

                _character = CreateCharacter();
                return _character;
            }
        }

        protected virtual void Update()
        {
            _character.Tick(Time.deltaTime);
        }

        protected abstract ICharacter CreateCharacter();
    }
}