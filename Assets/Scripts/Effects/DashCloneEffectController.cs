using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Effects
{
    public class DashCloneEffectController : MonoBehaviour
    {
        [SerializeField] private Sprite canvasSprite;
        [SerializeField, Min(0)] private float cloneLiveDuration = 0.2f;
        [SerializeField] private Color cloneColor = Color.red;
        [SerializeField, Min(0)] private float cloneAppearInterval = 0.1f;

        private float _timeRemained;

        private class CloneInfo
        {
            public GameObject Clone { get; }
            public float LifeTime { get; set; }

            public CloneInfo(GameObject clone, float lifeTime)
            {
                Clone = clone;
                LifeTime = lifeTime;
            }
        }

        private readonly List<CloneInfo> _clones = new List<CloneInfo>();

        private void Awake()
        {
            if (canvasSprite == null)
            {
                Debug.LogWarning("Canvas Sprite is not assigned", this);
                enabled = false;
            }
        
            _timeRemained = cloneAppearInterval;
        }

        public void Tick(Transform spriteTransform, Sprite dashSprite, float timeStep)
        {
            if (_timeRemained <= 0)
            {
                CreateEffectClone(spriteTransform, dashSprite);
                _timeRemained = cloneAppearInterval;
            }
            _timeRemained -= timeStep;
        }

        private void CreateEffectClone(Transform spriteTransform, Sprite dashSprite)
        {
            var cloneObject = new GameObject("Dash Effect Clone")
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            var cloneTransform = cloneObject.transform;
            cloneTransform.SetPositionAndRotation(spriteTransform.position, spriteTransform.rotation);
            cloneTransform.localScale = spriteTransform.localScale;

            cloneObject.AddComponent<SortingGroup>();

            var spriteRenderer = cloneObject.AddComponent<SpriteRenderer>();

            spriteRenderer.sprite = canvasSprite;
            spriteRenderer.color = cloneColor;
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

            var spriteMask = cloneObject.AddComponent<SpriteMask>();
            spriteMask.sprite = dashSprite;

            _clones.Add(new CloneInfo(cloneObject, cloneLiveDuration));
        }

        private void Update()
        {
            DecreaseCloneLifetimes(Time.deltaTime);
            DestroyDeadClones();
        }

        private void DecreaseCloneLifetimes(float timeStep)
        {
            foreach (var cloneInfo in _clones)
            {
                cloneInfo.LifeTime -= timeStep;
            }
        }

        private void DestroyDeadClones()
        {
            var toDestroy = _clones.Where(cloneInfo => cloneInfo.LifeTime <= 0).ToList();
            foreach (var cloneInfo in toDestroy)
            {
                _clones.Remove(cloneInfo);
                Destroy(cloneInfo.Clone);
            }
        }
    }
}
