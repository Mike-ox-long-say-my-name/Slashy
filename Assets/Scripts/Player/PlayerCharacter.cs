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
        public bool HasStamina => Stamina > 0;

        protected override void Awake()
        {
            base.Awake();
            Stamina = MaxStamina;
        }

        public void SpendStamina(float amount)
        {
            Stamina = Mathf.Max(0, Stamina - amount);
        }

        protected override void OnDeath()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
