using System;
using System.Collections.Generic;
using System.Linq;
using Core.Levels;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    public class CheatBoard : MonoBehaviour
    {
        private LazyPlayer _player;
        private IEnemyFactory _enemyFactory;

        private bool _godModeEnabled;
        private ISceneLoader _sceneLoader;
        private static bool _isCheatsEnabled;

        private static float _startHSpeed = -1;
        private static float _startVSpeed = -1;

        [SerializeField] private string cheatsEnableCombination = "slashycb";

        private readonly Stack<char> _remainedCombination = new Stack<char>();
        private IGameLoader _gameLoader;

        private void Awake()
        {
            _player = Container.Get<IPlayerFactory>().GetLazyPlayer();
            _enemyFactory = Container.Get<IEnemyFactory>();
            _sceneLoader = Container.Get<ISceneLoader>();
            _gameLoader = Container.Get<IGameLoader>();

            ResetCombination();

            if (_isCheatsEnabled)
            {
                Cursor.visible = true;
            }
        }

        private void ResetCombination()
        {
            _remainedCombination.Clear();
            foreach (var c in cheatsEnableCombination.Reverse())
            {
                _remainedCombination.Push(c);
            }
        }

        private void OnEnable()
        {
            Keyboard.current.onTextInput += OnTextInput;
        }

        private void OnDisable()
        {
            Keyboard.current.onTextInput -= OnTextInput;
        }

        private void OnTextInput(char obj)
        {
            if (_isCheatsEnabled)
            {
                return;
            }

            if (_remainedCombination.Peek() == obj)
            {
                _remainedCombination.Pop();
                if (_remainedCombination.Count == 0)
                {
                    Cursor.visible = true;
                    Debug.Log("Cheats enabled.");
                    _isCheatsEnabled = true;
                }
            }
            else
            {
                ResetCombination();
            }
        }

        private void OnGUI()
        {
            if (!_isCheatsEnabled)
            {
                return;
            }

            var style = new GUIStyle();
            style.normal.background = Texture2D.grayTexture;

            GUILayout.BeginArea(new Rect(Screen.width - 160, 0, 160, 320), style);

            var newGodModeState = GUILayout.Toggle(_godModeEnabled, "God Mode");
            if (newGodModeState != _godModeEnabled)
            {
                SetGodModeState(newGodModeState);
                Debug.Log($"God Mode {(newGodModeState ? "enabled" : "disabled")}.");
            }

            var currentSpeed = _player.Value.VelocityMovement.HorizontalSpeed;
            GUILayout.Label("Player Speed: " + currentSpeed.ToString("F1"));
            var speedValue = GUILayout.HorizontalSlider(currentSpeed,
                0, 15);
            SetPlayerSpeed(speedValue, speedValue);

            if (GUILayout.Button("Respawn All Enemies"))
            {
                _enemyFactory.RecreateAllEnemiesOnLevel();

                Debug.Log("Enemies respawned.");
            }

            if (GUILayout.Button("Destroy All Enemies"))
            {
                _enemyFactory.DestroyAllCreated();

                Debug.Log("Enemies destroyed.");
            }

            GUILayout.Label("Go to Level:");

            for (var i = -1; i < LevelLibrary.LevelCount; i++)
            {
                var level = i == -1 ? LevelLibrary.Menu : LevelLibrary.GetLevel(i);
                if (GUILayout.Button(level))
                {
                    _sceneLoader.ReplaceLastScene(level);
                    break;
                }
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Complete Game"))
            {
                _gameLoader.CompleteGame();
            }
            
            GUILayout.Space(10);

            if (GUILayout.Button("Disable Cheats"))
            {
                SetGodModeState(false);
                SetPlayerSpeed(_startHSpeed, _startVSpeed);
                
                _isCheatsEnabled = false;

                Cursor.visible = false;
                ResetCombination();

                Debug.Log("Cheats disabled.");
            }

            GUILayout.EndArea();
        }

        private void SetGodModeState(bool state)
        {
            _godModeEnabled = state;
            _player.Value.Character.Health.Frozen = _godModeEnabled;
            _player.Value.Character.Balance.Frozen = _godModeEnabled;
            _player.Value.Stamina.Frozen = _godModeEnabled;
        }

        private void SetPlayerSpeed(float hspeed, float vspeed)
        {
            if (Math.Abs(_startHSpeed - (-1)) < 1e-5)
            {
                _startHSpeed = _player.Value.VelocityMovement.HorizontalSpeed;
            }

            if (Math.Abs(_startVSpeed - (-1)) < 1e-5)
            {
                _startVSpeed = _player.Value.VelocityMovement.VerticalSpeed;
            }

            _player.Value.VelocityMovement.HorizontalSpeed = hspeed;
            _player.Value.VelocityMovement.VerticalSpeed = vspeed;
        }
    }
}