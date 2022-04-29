using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Characters.Mono;
using UnityEngine;

namespace Effects
{
    public abstract class BaseCharacterHitListener : MonoBehaviour
    {
        protected virtual void Subscribe()
        {
            var monoCharacter = GetComponentInParent<MonoCharacter>();
            if (monoCharacter == null)
            {
                return;
            }

            var character = monoCharacter.Character;
            character.OnHitReceived += OnHitReceived;
        }

        protected virtual void Unsubscribe()
        {
            var monoCharacter = GetComponentInParent<MonoCharacter>();
            if (monoCharacter == null)
            {
                return;
            }

            var character = monoCharacter.Character;
            character.OnHitReceived -= OnHitReceived;
        }

        protected abstract void OnHitReceived(IHitReceiver entity, HitInfo info);
    }
}