using Core;
using Core.Attacking.Mono;
using Core.Player.Interfaces;
using UnityEngine;

namespace UI.PopupHints
{
    [RequireComponent(typeof(MixinTriggerEventDispatcher))]
    public class PopupTriggerEvent : MonoBehaviour
    {
        [SerializeField] private float showDelay;
        [SerializeField] private PopupHintType hintType;
        [SerializeField] private MixinTriggerEventDispatcher triggerEvents;

        private IPopupHintController _popupHintController;
        private ICoroutineRunner _coroutineRunner;
        private bool _entered;

        private void Reset()
        {
            triggerEvents = GetComponent<MixinTriggerEventDispatcher>();
        }

        private void Awake()
        {
            _coroutineRunner = Container.Get<ICoroutineRunner>();
            _popupHintController = Container.Get<IPopupHintController>();
            triggerEvents.Enter.AddListener(OnShowHintTrigger);
        }

        private void OnShowHintTrigger(Collider obj)
        {
            if (!ShouldShowHint(obj))
            {
                return;
            }

            _entered = true;
            ScheduleHintPopup();
        }

        private void ScheduleHintPopup()
        {
            _coroutineRunner.RunAfter(() => _popupHintController.ShowHint(hintType), showDelay);
        }

        private bool ShouldShowHint(Collider obj)
        {
            return !_entered && obj.GetComponent<IPlayer>() != null;
        }
    }
}