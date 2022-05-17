using UnityEngine;

namespace Core.Levels
{
    public class ExitManager : PublicSingleton<ExitManager>
    {
        [SerializeField] private ExitData exitData;

        //private void Start()
        //{
        //    return;

        //    var gameLoader = GameLoader.Instance;
        //    gameLoader.Exiting.AddListener(OnExiting);
        //    gameLoader.LoadedExitedLevel.AddListener(OnLoadedExitedLevel);
        //    gameLoader.LoadedLevel.AddListener(OnLoadedLevel);
        //    gameLoader.StartingNewGame.AddListener(OnStartingNewGame);

        //    PlayerManager.Instance.PlayedDeadSequenceStarted.AddListener(OnPlayedDied);
        //}

        //private void OnLoadedLevel(string arg0)
        //{
        //    // Если мы перезагружаем уровень из-за смерти, то нужно сбросить флаг того, что игрок мертв
        //    exitData.Dead = false;
        //}

        //private void OnStartingNewGame(string level)
        //{
        //    exitData.ExitPosition = Vector3.zero;
        //    exitData.Dead = false;
        //}

        //private void OnLoadedExitedLevel(string level)
        //{
        //    PlayerManager.Instance.PlayerLoaded.AddListener(OnPlayerLoaded);
        //}

        //private void OnPlayerLoaded()
        //{
        //    // Происходит только при продолжении игры
        //    PlayerManager.Instance.PlayerLoaded.RemoveListener(OnPlayerLoaded);

        //    var wasDead = exitData.Dead;
        //    exitData.Dead = false;

        //    if (!wasDead)
        //    {
        //        ApplyCurrentPlayerData();
        //        ApplyExitPosition();
        //    }
        //    else
        //    {
        //        SaveCurrentPlayerData();
        //    }
        //}

        //public void OnExiting(string level)
        //{
        //    exitData.Level = level;
        //    SaveCurrentPlayerData();
        //}

        //private void OnPlayedDied()
        //{
        //    exitData.Dead = true;
        //}

        //public void SaveCurrentPlayerData()
        //{
        //    var player = PlayerManager.Instance.PlayerInfo;
        //    exitData.Stamina = player.Player.Stamina.Value;
        //    exitData.Health = player.Player.Character.Health.Value;
        //    exitData.Balance = player.Player.Character.Balance.Value;
        //    exitData.ExitPosition = player.Transform.position;
        //}

        //public void ApplyCurrentPlayerData()
        //{
        //    var player = PlayerManager.Instance.PlayerInfo;
        //    player.Player.Stamina.Value = exitData.Stamina;
        //    player.Player.Character.Health.Value = exitData.Health;
        //    player.Player.Character.Balance.Value = exitData.Balance;
        //}

        //public void ApplyExitPosition()
        //{
        //    var player = PlayerManager.Instance.PlayerInfo;
        //    player.VelocityMovement.BaseMovement.SetPosition(exitData.ExitPosition);
        //}
    }
}