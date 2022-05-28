using System.Linq;
using UnityEngine;

namespace UI.PopupHints
{
    public class PopupHintController : IPopupHintController
    {
        private readonly PopupHintContainer _hintContainer;
        private PopupHint _openHint;

        public PopupHintController(PopupHintContainer hintContainer)
        {
            _hintContainer = hintContainer;
        }

        public void ShowHint(PopupHintType hintType)
        {
            var desc = _hintContainer.HintDescriptions
                .FirstOrDefault(desc => desc.type == hintType);
            if (desc.popupHint == null)
            {
                Debug.LogError($"Where is not hint for type {hintType}");
                return;
            }

            HideOpenedHintIfAny();
            ShowHintFromDescription(desc);
        }

        private void ShowHintFromDescription(PopupHintDescription desc)
        {
            _openHint = desc.popupHint;
            _openHint.Show();
            _hintContainer.ShownHints.MarkAsShown(desc.type);
        }

        private void HideOpenedHintIfAny()
        {
            if (_openHint != null)
            {
                _openHint.Hide();
            }
        }
    }
}