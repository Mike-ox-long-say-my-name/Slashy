using Core;
using System.Collections;
using UnityEngine;

namespace UI.PopupHints
{
    [RequireComponent(typeof(MixinInteractable))]
    [RequireComponent(typeof(RectTransform))]
    public class PopupHint : MonoBehaviour
    {
        private RectTransform _transform;

        [SerializeField] private Vector2 appearMove;
        [SerializeField, Min(0.01f)] private float appearTime = 0.55f;

        private bool _isShown = false;
        private MixinInteractable _interactable;

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
            _interactable = GetComponent<MixinInteractable>();
        }

        public void Show()
        {
            if (_isShown)
            {
                return;
            }

            _interactable.MakeInteractable();
            _isShown = true;
            StartCoroutine(MoveRoutine(appearMove, appearTime));
        }

        public void Hide()
        {
            if (!_isShown)
            {
                return;
            }

            _interactable.MakeUninteractable();
            _isShown = false;
            StartCoroutine(MoveRoutine(-appearMove, appearTime));
        }

        private IEnumerator MoveRoutine(Vector2 move, float time)
        {
            var passedTime = 0f;
            var startPosition = _transform.anchoredPosition;
            while (passedTime < time)
            {
                passedTime = Mathf.Min(passedTime + Time.deltaTime, time);
                var moveStep = move * (passedTime / time);
                _transform.anchoredPosition = startPosition + moveStep;
                yield return null;
            }
        }
    }
}
