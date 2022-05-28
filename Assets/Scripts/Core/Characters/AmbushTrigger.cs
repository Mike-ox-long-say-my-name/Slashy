using Core.Attacking.Mono;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(MixinTriggerEventDispatcher))]
    public class AmbushTrigger : MonoBehaviour
    {
        [SerializeField] private Ambush[] ambushes;

        private void Awake()
        {
            var triggerEvents = GetComponent<MixinTriggerEventDispatcher>();
            triggerEvents.Enter.AddListener(_ => Activate());
        }

        public void Activate()
        {
            foreach (var ambush in ambushes)
            {
                ambush.ActivateAmbush();
            }
        }
    }
}