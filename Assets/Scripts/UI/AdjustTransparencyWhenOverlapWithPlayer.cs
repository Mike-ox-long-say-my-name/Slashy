using System;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{   
    [RequireComponent(typeof(RectTransform))]
    public class AdjustTransparencyWhenOverlapWithPlayer : MonoBehaviour
    {
        [SerializeField] private Image adjustedImage;
        [SerializeField] private new Camera camera;
        [SerializeField] private Vector2 playerExtents;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private float overlappedAlpha = 0.3f;
        [SerializeField] private float transitionSpeed = 1;

        private LazyPlayer _lazyPlayer;
        private float _initialAlpha;

        private float _currentTime01;
        private float _transition;

        private void Reset()
        {
            rectTransform = GetComponent<RectTransform>();
            adjustedImage = GetComponent<Image>();
            camera = Camera.main;
        }

        private void Awake()
        {
            _lazyPlayer = Container.Get<IPlayerFactory>().GetLazyPlayer();
            _initialAlpha = adjustedImage.color.a;
            _transition = overlappedAlpha - _initialAlpha;

            if (camera == null)
            {
                camera = Camera.main;
            }
        }

        private void LateUpdate()
        {
            if (!_lazyPlayer.IsCreated)
            {
                return;
            }

            var playerRect = GetPlayerRect();
            var imageRect = GetWorldRect();
            var targetAlpha = GetTargetAlpha(imageRect, playerRect);

            SetAlpha(targetAlpha);
        }

        private float GetTargetAlpha(Rect imageRect, Rect playerRect)
        {
            var targetAlpha = imageRect.Overlaps(playerRect) ? overlappedAlpha : _initialAlpha;
            return targetAlpha;
        }

        private Rect GetPlayerRect()
        {
            var player = _lazyPlayer.Value;
            var worldPosition = player.Transform.position;
            Vector2 screenPosition = camera.WorldToScreenPoint(worldPosition);
            var playerRect = new Rect(screenPosition - playerExtents, playerExtents);
            return playerRect;
        }

        private void SetAlpha(float value)
        {
            var color = adjustedImage.color;
            var sign = Math.Sign(color.a - value);

            _currentTime01 = Mathf.Clamp01(_currentTime01 + sign * transitionSpeed * Time.deltaTime);
            
            color.a = _initialAlpha + _transition * _currentTime01;
            adjustedImage.color = color;
        }

        private Rect GetWorldRect()
        {
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            var position = corners[0];

            var rect = rectTransform.rect;
            var lossyScale = rectTransform.lossyScale;
            var size = new Vector2(
                lossyScale.x * rect.size.x,
                lossyScale.y * rect.size.y);

            return new Rect(position, size);
        }
    }
}