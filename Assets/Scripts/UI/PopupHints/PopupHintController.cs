using Core.Player;
using Core.Player.Interfaces;
using UnityEngine;

namespace UI.PopupHints
{
    public class PopupHintController : MonoBehaviour
    {
        [SerializeField] private ShownHintsSO shownHints;

        [Header("Hints")]
        [SerializeField] private PopupHint moveHint;
        [SerializeField] private PopupHint dashHint;
        [SerializeField] private PopupHint jumpHint;
        [SerializeField] private PopupHint lightAttackHint;
        [SerializeField] private PopupHint jumpAttackHint;
        [SerializeField] private PopupHint strongAttackHint;
        [SerializeField] private PopupHint healHint;

        private PopupHint _openHint;

        private static IPlayer Player => PlayerManager.Instance.PlayerInfo;

        private void Start()
        {
            SubscribeToHintEvents();
            ReadPlayerCapabilities();

            Invoke(nameof(ShowMoveHint), 0.5f);
        }

        private void ReadPlayerCapabilities()
        {
            var capabilities = Player.Capabilities;
            capabilities.CanDash = shownHints.Dash;
            capabilities.CanJump = shownHints.Jump;
            capabilities.CanHeal = shownHints.Heal;
            capabilities.CanLightAttack = shownHints.LightAttack;
            capabilities.CanStrongAttack = shownHints.StrongAttack;
        }

        public void ShowMoveHint()
        {
            if (shownHints.Move)
            {
                return;
            }

            TryShowHint(moveHint);
            shownHints.Move = true;
        }

        public void ShowDashHint()
        {
            if (shownHints.Dash)
            {
                return;
            }

            TryShowHint(dashHint);
            Player.Capabilities.CanDash = true;

            shownHints.Dash = true;
        }

        public void ShowLightAttackHint()
        {
            if (shownHints.LightAttack)
            {
                return;
            }

            TryShowHint(lightAttackHint);
            Player.Capabilities.CanLightAttack = true;

            shownHints.LightAttack = true;
        }

        public void ShowStrongAttackHint()
        {
            if (shownHints.StrongAttack)
            {
                return;
            }

            TryShowHint(strongAttackHint);
            Player.Capabilities.CanStrongAttack = true;

            shownHints.StrongAttack = true;
        }

        private void SubscribeToHintEvents()
        {
            // TODO: добавить подписку на события
        }

        private void TryShowHint(PopupHint hint)
        {
            if (_openHint != null)
            {
                _openHint.Hide();
            }

            _openHint = hint;
            if (hint != null)
            {
                hint.Show();
                return;
            }

            Debug.LogWarning("Hint is null", this);
        }
    }
}