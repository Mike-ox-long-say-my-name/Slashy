using Core;
using System.Collections;
using UnityEngine;

namespace UI.PopupHints
{
    [RequireComponent(typeof(RectTransform))]
    public class PopupHint : AbstractInteractable
    {
        private RectTransform _transform;

        [SerializeField] private Vector2 appearMove;
        [SerializeField, Min(0.01f)] private float appearTime = 0.55f;

        private bool _isShown = false;

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
        }

        public void Show()
        {
            if (_isShown)
            {
                return;
            }

            MakeInteractable();
            _isShown = true;
            StartCoroutine(MoveRoutine(appearMove, appearTime));
        }

        public void Hide()
        {
            if (!_isShown)
            {
                return;
            }

            MakeUninteractable();
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

        protected override object InteractInternal()
        {
            Hide();
            return this;
        }
    }
}
