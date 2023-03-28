using System;
using UnityEngine;

namespace UnityGame.MVC
{
    public class InputView : MonoBehaviour, IView
    {
        [SerializeField] private InputController _controller;

        public Vector2 GetMoveDirection()
        {
            return _controller.MoveInput;
        }

        public void RegisterKeyDown(KeyCode key, Action action)
        {
            _controller.RegisterKeyDown(key, action);
        }

        public void UnregisterKeyDown(KeyCode key, Action action)
        {
            _controller.UnregisterKeyDown(key, action);
        }
    }
}

