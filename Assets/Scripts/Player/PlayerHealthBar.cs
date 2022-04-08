using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Image frontLayerImage;
    [SerializeField] private Image middleLayerImage;
    [SerializeField, Range(0, 0.1f)] private float fillAnimationPerTick = 0.05f;
    [SerializeField, Min(0)] private float fillAnimationTickInterval = 0.05f;
    [SerializeField, Range(0, 1)] private float fillAnimationStartDelay = 0.15f;

    private void Awake()
    {
        var player = FindObjectOfType<PlayerCharacter>();
        player.onHitReceived.AddListener(OnPlayerHitReceived);
    }

    public void OnPlayerHitReceived(PlayerCharacter player)
    {
        var fill = player.Health / player.MaxHealth;
        frontLayerImage.fillAmount = fill;

        if (_middleLayerAnimation != null)
        {
            StopCoroutine(_middleLayerAnimation);
        }
        _middleLayerAnimation = StartCoroutine(MiddleLayerAnimation(fill));
    }

    private IEnumerator MiddleLayerAnimation(float targetFill)
    {
        if (fillAnimationPerTick < 1e-8)
        {
            yield break;
        }

        if (fillAnimationStartDelay > 0)
        {
            yield return new WaitForSeconds(fillAnimationStartDelay);
        }

        var fill = middleLayerImage.fillAmount;
        while (fill > targetFill)
        {
            fill = Mathf.Max(targetFill, fill - fillAnimationPerTick);
            middleLayerImage.fillAmount = fill;
            yield return new WaitForSeconds(fillAnimationTickInterval);
        }
    }

    private Coroutine _middleLayerAnimation;
}
