using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGame.MVC
{
    class UIFactory : MonoBehaviour, IValueGetter<IView>, IValueGetter<IController>
    {
        [SerializeField] private Transform _spawnedObjectsContainer;
        private IObjectStorage<IView> _viewStorage;
        private IObjectStorage<IController> _controllerStorage;

        public void Initialize(IObjectStorage<IView> viewStorage, IObjectStorage<IController> controllerStorage)
        {
            _viewStorage = viewStorage;
            _controllerStorage = controllerStorage;
        }

        public IScreen CreateScreen(IScreen prefab)
        {
            UIScreen prefabReal = prefab as UIScreen;

            UIScreen screen = Instantiate(prefabReal, _spawnedObjectsContainer);

            InjectReferences(screen);

            return screen;
        }

        private void InjectReferences(UIScreen screen)
        {
            FieldInjector injector = new FieldInjector();
            injector.InjectReferences<IView>(screen, this);
            injector.InjectReferences<IController>(screen, this);
        }

        IController IValueGetter<IController>.GetValue(Type type)
        {
            return _controllerStorage.GetValue(type);
        }

        IView IValueGetter<IView>.GetValue(Type type)
        {
            return _viewStorage.GetValue(type);
        }
    }
}
