using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGame.MVC
{
    public class InputController : MonoBehaviour, IController
    {
        public Vector2 MoveInput = Vector2.zero;
        private readonly Dictionary<KeyCode, Vector2> _directionKeys = new Dictionary<KeyCode, Vector2>()
        {
            {KeyCode.W, new Vector2(0, 1)},
            {KeyCode.S, new Vector2(0, -1)},
            {KeyCode.A, new Vector2(-1, 0)},
            {KeyCode.D, new Vector2(1, 0)},
        };
        private Dictionary<KeyCode, Action> _listeners = new Dictionary<KeyCode, Action>();

        public void RegisterKeyDown(KeyCode key, Action action)
        {
            if (!_listeners.ContainsKey(key))
            {
                _listeners[key] = () => { };
            }
            _listeners[key] += action;
        }

        public void UnregisterKeyDown(KeyCode key, Action action)
        {
            if (_listeners.ContainsKey(key))
            {
                _listeners[key] -= action;
            }
        }

        private void Update()
        {
            ProcessKeyDown();
            ProcessKeyUp();
        }

        private void ProcessKeyDown()
        {
            foreach(var pair in _directionKeys)
            {
                if (Input.GetKeyDown(pair.Key))
                {
                    MoveInput.x += pair.Value.x;
                    MoveInput.y += pair.Value.y;
                }
            }

            foreach (var pair in _listeners)
            {
                if (Input.GetKeyDown(pair.Key))
                {
                    _listeners[pair.Key]?.Invoke();
                }
            }
        }

        private void ProcessKeyUp()
        {
            foreach (var pair in _directionKeys)
            {
                if (Input.GetKeyUp(pair.Key))
                {
                    MoveInput.x -= pair.Value.x;
                    MoveInput.y -= pair.Value.y;
                }
            }
        }
    }
}

