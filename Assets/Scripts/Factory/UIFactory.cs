using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGame.MVC
{
    class UIFactory : MonoBehaviour
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
            List<FieldInfo> views = GetFieldsOfType<IView>(screen);
            foreach (var field in views)
            {
                IView view = _viewStorage.GetValue(field.FieldType);
                field.SetValue(screen, view);
                Debug.Log($"[UIFactory] Injected View. Field:{field.Name}; View:{view.GetType()}");
            }

            List<FieldInfo> controllers = GetFieldsOfType<IController>(screen);
            foreach (var field in controllers)
            {
                IController controller = _controllerStorage.GetValue(field.FieldType);
                field.SetValue(screen, controller);
                Debug.Log($"[UIFactory] Injected Controller. Field:{field.Name}; Controller:{controller.GetType()}");
            }
        }

        private List<FieldInfo> GetFieldsOfType<T>(object obj)
        {
            List<FieldInfo> result = new List<FieldInfo>();
            Type neededFieldType = typeof(T);
            Type objectType = obj.GetType();
            foreach (FieldInfo field in objectType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                bool isValid = field.FieldType == neededFieldType || neededFieldType.IsAssignableFrom(field.FieldType);
                if (isValid)
                {
                    result.Add(field);
                }
            }
            return result;
        }
    }
}
