using System.Linq;
using Core;
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
            if (IsShown(hintType))
            {
                return;
            }
            
            var desc = GetHintDescription(hintType);
            if (desc.popupHint == null)
            {
                Debug.LogError($"Where is not hint for type {hintType}");
                return;
            }
            
            HideOpenedHintIfAny();
            ShowHintFromDescription(desc);
        }

        private PopupHintDescription GetHintDescription(PopupHintType hintType)
        {
            return _hintContainer.HintDescriptions
                .FirstOrDefault(desc => desc.type == hintType);
        }

        private bool IsShown(PopupHintType hintType)
        {
            return _hintContainer.ShownHints.IsShown(hintType);
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