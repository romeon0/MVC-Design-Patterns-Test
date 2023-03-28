using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGame.MVC
{
    public class GameplayView : MonoBehaviour, IView
    {
        [SerializeField] private GameplayController _controller;
        [SerializeField] private InputView _inputView;
        [SerializeField] private GameplayFactory _factory;
        private bool _objectsSpawnDone;
        private List<GameObject> _spawnedObjects = new List<GameObject>();

        public Player Player { get; private set; }
        public int CurrentHealth => _controller.UnitController.GetCurrentHealth();
        public int TotalHealth => _controller.UnitController.GetTotalHealth();
        public float CurrentTime => _controller.GetCurrentTime();
        public bool GameRunning => _controller.GameRunning && _objectsSpawnDone;

        public Action<int, int, int> PlayerDamaged;
        public Action GameStarted;
        public Action<GameEndReason> GameFinished;
        public Action<bool> GamePausedChanged;

        public void StartGame()
        {
            _controller.StartGame();

            Player = _factory.CreatePlayer();
            Player.Clicked += OnPlayerClicked;
            _spawnedObjects.Add(Player.gameObject);

            _objectsSpawnDone = true;
        }

        public void StopGame()
        {
            _controller.StopGame(GameEndReason.ExitByUser);
        }

        internal void ResumeGame()
        {
            _controller.PauseGame(false);
        }

        internal void PauseGame()
        {
            _controller.PauseGame(true);
        }

        private void Clear()
        {
            _objectsSpawnDone = false;
            if (Player != null)
            {
                Player.Clicked -= OnPlayerClicked;
                Destroy(Player.gameObject);
                Player = null;
            }
            _spawnedObjects.Clear();
        }

        private void Start()
        {
            _controller.UnitController.PlayerDamaged += OnPlayerDamaged;
            _controller.GameStarted += OnGameStarted;
            _controller.GameFinished += OnGameFinished;
            _controller.GamePauseChanged += OnGamePauseChanged;
        }

        private void OnPlayerClicked()
        {
            _controller.UnitController.TakeDamage(20);
        }

        private void Update()
        {
            if (!GameRunning)
            {
                return;
            }

            Vector2 moveDir = _inputView.GetMoveDirection();

            if (moveDir == Vector2.zero)
            {
                return;
            }

            Player.transform.position += new Vector3(moveDir.x, moveDir.y, 0);
        }

        #region Controller's Events
        private void OnPlayerDamaged(int damage)
        {
            PlayerDamaged?.Invoke(damage, CurrentHealth, TotalHealth);
        }

        private void OnGameStarted()
        {
            GameStarted?.Invoke();
        }

        private void OnGameFinished(GameEndReason reason)
        {
            Clear();
            GameFinished?.Invoke(reason);
        }

        private void OnGamePauseChanged(bool paused)
        {
            foreach(var obj in _spawnedObjects)
            {
                obj.SetActive(!paused);
            }
            GamePausedChanged?.Invoke(paused);
        }
        #endregion
    }
}

