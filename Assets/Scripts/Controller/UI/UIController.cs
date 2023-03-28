using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGame.MVC
{
    public class UIController : MonoBehaviour, IUIController
    {
        [SerializeField] private UIModel _model;
        [SerializeField] private UIFactory _factory;
        private Dictionary<Type, IScreen> _spawnedScreens = new Dictionary<Type, IScreen>();

        public void Show<T>() where T: IScreen
        {
            Type type = typeof(T);

            IScreen screen;

            if(!_spawnedScreens.TryGetValue(type, out screen))
            {
                IScreen prefab = _model.prefabs.Find(e => e.GetType() == type);
                screen = _factory.CreateScreen(prefab);
                screen.Initialize(this);
                _spawnedScreens.Add(type, screen);
            }
        
            screen.Show();
        }

        public void Hide<T>() where T : IScreen
        {
            IScreen screen = GetScreen<T>();
            screen.Hide();
        }

        public IScreen GetScreen<T>() where T : IScreen
        {
            Type type = typeof(T);
            IScreen screen = _spawnedScreens[type];
            return screen;
        }
    }
}

