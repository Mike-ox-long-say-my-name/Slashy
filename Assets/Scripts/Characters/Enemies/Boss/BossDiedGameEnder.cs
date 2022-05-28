using Core;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    public class BossDiedGameEnder : MonoBehaviour
    {
        private IGameEndedSequencePlayer _gameEndSequencePlayer;

        private void Awake()
        {
            Construct();
        }

        private void Start()
        {
            var boss = FindObjectOfType<BossMarker>().BossEvents;
            boss.Died.AddListener(OnBossDied);
        }

        private void Construct()
        {
            _gameEndSequencePlayer = Container.Get<IGameEndedSequencePlayer>();
        }

        private void OnBossDied()
        {
            _gameEndSequencePlayer.Play();
        }
    }
}