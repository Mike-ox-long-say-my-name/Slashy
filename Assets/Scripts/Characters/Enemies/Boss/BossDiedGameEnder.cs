using Core;
using Core.Characters;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    public class BossDiedGameEnder : MonoBehaviour
    {
        [SerializeField] private MixinBossEventDispatcher bossEvents;
        private IGameEndedSequencePlayer _gameEndSequencePlayer;

        private void Awake()
        {
            Construct();
        }

        private void Construct()
        {
            bossEvents.Died.AddListener(OnBossDied);

            _gameEndSequencePlayer = Container.Get<IGameEndedSequencePlayer>();
        }

        private void OnBossDied()
        {
            _gameEndSequencePlayer.Play();
        }
    }
}