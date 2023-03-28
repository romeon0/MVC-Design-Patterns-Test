using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGame.MVC
{
    public class UIPauseScreen : UIScreen
    {
        [SerializeField] private Button _resume;
        [SerializeField] private Button _exit;
        private GameplayView _view;

        private void Start()
        {
            _resume.onClick.AddListener(OnResumeClicked);
            _exit.onClick.AddListener(OnExitClicked);
        }

        protected override void ShowInternal()
        {
            _view.GamePausedChanged += OnGamePausedChanged;
            _view.GameFinished += OnGameFinished;
        }

        protected override void HideInternal()
        {
            _view.GamePausedChanged -= OnGamePausedChanged;
            _view.GameFinished -= OnGameFinished;
        }

        private void OnResumeClicked()
        {
            _view.ResumeGame();
        }

        private void OnExitClicked()
        {
            _view.StopGame();
        }

        #region View's Events
        private void OnGamePausedChanged(bool paused)
        {
            if (!paused)
            {
                _uiController.Hide<UIPauseScreen>();
                _uiController.Show<UIGameplayScreen>();
            }
        }

        private void OnGameFinished(GameEndReason reason)
        {
            _uiController.Hide<UIPauseScreen>();
            _uiController.Show<UIMainScreen>();
        }
        #endregion
    }
}

