using System.Collections;
using Core;
using Core.Characters;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    public class BossDiedGameEnder : MonoBehaviour
    {
        [SerializeField] private MixinBossEventDispatcher bossEvents;
        [SerializeField] private float waitAfterDeathTime = 5;
        [SerializeField] private float blackoutTime = 5f;
        
        private void Start()
        {
            bossEvents.Died.AddListener(OnBossDied);
        }

        private void OnBossDied()
        {
            StartCoroutine(PlayGameEndSequence());
        }

        private IEnumerator PlayGameEndSequence()
        {
            yield return new WaitForSeconds(waitAfterDeathTime);

            var manager = BlackScreenManager.Instance;
            manager.Blackout(blackoutTime);

            yield return new WaitForSeconds(blackoutTime);
            
            AggroListener.Instance.ResetAggroCounter();
            GameLoader.Instance.CompleteGame();
        }
    }
}