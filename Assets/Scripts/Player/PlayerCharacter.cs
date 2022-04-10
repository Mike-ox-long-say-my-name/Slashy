using Attacking;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerCharacter : Character
    {
        [field: SerializeField, Min(0)] public float MaxStamina { get; private set; } = 0;

        public float Stamina { get; private set; } = 0;

        public UnityEvent<PlayerCharacter> onHitReceived;

        public bool HasStamina => Stamina > 0;

        protected override void Awake()
        {
            base.Awake();
            Stamina = MaxStamina;
        }

        public override void ReceiveHit(IHitSource source, in HitInfo info)
        {
            base.ReceiveHit(source, info);
            onHitReceived?.Invoke(this);

            print("Ouch");
        }

        public void SpentStamina(float amount)
        {
            Stamina = Mathf.Max(0, Stamina - amount);
        }

        protected override void OnDeath()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
