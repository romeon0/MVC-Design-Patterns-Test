using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGame.MVC
{
    public enum GameEndReason
    {
        ExitByUser = 0,
        PlayerDied = 1,
        Win = 2
    }

    public class GameplayController : MonoBehaviour, IController
    {
        [SerializeField] private float _matchTime;
        [SerializeField] private UnitController _unitController;
        private float _time;
        private bool _paused;

        public bool GameRunning { get; private set; }
        public UnitController UnitController => _unitController;

        public Action GameStarted;
        public Action<GameEndReason> GameFinished;
        public Action<bool> GamePauseChanged;

        private void Start()
        {
            _unitController.PlayerDied += OnPlayerDied;
        }

        public void StartGame()
        {
            if (GameRunning)
            {
                Debug.LogError("Already running!");
                return;
            }

            _unitController.Init();

            _time = _matchTime;
            _paused = false;

            GameRunning = true;

            GameStarted?.Invoke();
        }

        public void StopGame(GameEndReason reason)
        {
            if (!GameRunning)
            {
                Debug.LogError("Not running!");
                return;
            }

            _time = 0f;
            GameRunning = false;

            GameFinished?.Invoke(reason);
        }

        internal void PauseGame(bool paused)
        {
            if (paused == _paused)
            {
                if (_paused)
                {
                    Debug.LogError("Already paused!");
                }
                else
                {
                    Debug.LogError("Not paused!");
                }
                return;
            }

            _paused = paused;

            GamePauseChanged?.Invoke(paused);
        }

        internal float GetCurrentTime()
        {
            return _time;
        }

        private void Update()
        {
            if(!GameRunning || _paused)
            {
                return;
            }

            if(_time > 0)
            {
                _time -= Time.deltaTime;
                if(_time <= 0)
                {
                    StopGame(GameEndReason.Win);
                }
            }
        }



        private void OnPlayerDied()
        {
            StopGame(GameEndReason.PlayerDied);
        }
    }
}

