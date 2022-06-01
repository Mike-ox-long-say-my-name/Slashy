using System.Collections;
using UnityEngine;

namespace Characters.Player
{
    public class AwarenessIcon : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer actualIcon;
        [SerializeField] private SpriteRenderer transparentCopy;

        [ContextMenu("Play Animation")]
        public void PlayAnimation()
        {
            StopAllCoroutines();
            StartCoroutine(IconRoutine());
        }

        private IEnumerator IconRoutine()
        {
            var copy = transparentCopy.transform;
            actualIcon.enabled = true;
            transparentCopy.enabled = true;

            var passedTime = 0f;
            while (passedTime < 1)
            {
                var deltaTime = Time.deltaTime;
                copy.localScale += Vector3.one * deltaTime;
                passedTime += deltaTime;

                yield return null;
            }
            
            actualIcon.enabled = false;
            transparentCopy.enabled = false;

            copy.localScale = Vector3.one;
        }
    }
}