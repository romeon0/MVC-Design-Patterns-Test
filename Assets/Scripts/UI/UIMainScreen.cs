using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGame.MVC
{
    public class UIMainScreen : UIScreen 
    {
        [SerializeField] private Button _startGame;
        private GameplayView _view;

        private void Start()
        {
            _startGame.onClick.AddListener(OnStartGameClicked);
        }

        protected override void ShowInternal()
        {
            _view.GameStarted += OnGameStarted;
        }

        protected override void HideInternal()
        {
            _view.GameStarted -= OnGameStarted;
        }

        private void OnStartGameClicked()
        {
            _view.StartGame();
        }


        #region View's Events
        private void OnGameStarted()
        {
            _uiController.Hide<UIMainScreen>();
            _uiController.Show<UIGameplayScreen>();
        }
        #endregion
    }
}

