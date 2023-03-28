using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGame.MVC
{
    public class UIGameplayScreen : UIScreen
    {
        [SerializeField] private Text _health;
        [SerializeField] private Text _time;
        [SerializeField] private Text _gameRunning;
        [SerializeField] private Button _pause;
        private GameplayView _view;
        private InputView _inputView;

        private void Start()
        {
            _pause.onClick.AddListener(OnPauseClicked);
        }

        protected override void ShowInternal()
        {
            _view.PlayerDamaged += OnPlayerDamaged;
            _view.GameFinished += OnGameFinished;
            _inputView.RegisterKeyDown(KeyCode.I, OnOpenInventoryRequest);


            _gameRunning.text = "Running: true";
            _health.text = $"Health: {_view.CurrentHealth}/{_view.TotalHealth}";
        }

        protected override void HideInternal()
        {
            _view.PlayerDamaged -= OnPlayerDamaged;
            _view.GameFinished -= OnGameFinished;
            _inputView.UnregisterKeyDown(KeyCode.I, OnOpenInventoryRequest);
        }

        private void Clear()
        {
            _health.text = $"--/--";
            _time.text = $"--";
            _gameRunning.text = "Running: false";
        }

        private void Update()
        {
            if (!_initialized)
            {
                return;
            }

            if (_view.GameRunning)
            {
                _time.text = $"Time: {_view.CurrentTime:00.00}";
            }
        }

        private void OnPauseClicked()
        {
            _view.PauseGame();
            _uiController.Show<UIPauseScreen>();
        }

        #region View's Events
        private void OnPlayerDamaged(int damage, int currentHealth, int totalHealth)
        {
            _health.text = $"Health: {currentHealth}/{totalHealth}";
        }

        private void OnGameFinished(GameEndReason reason)
        {
            Clear();
            _uiController.Hide<UIGameplayScreen>();
            _uiController.Show<UIMainScreen>();
        }

        private void OnOpenInventoryRequest()
        {
            _uiController.Show<UIInventoryScreen>();
        }
        #endregion
    }
}

