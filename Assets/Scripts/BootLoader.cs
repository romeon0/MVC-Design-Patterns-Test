using UnityEngine;
using UnityGame.Tests;

namespace UnityGame.MVC
{
    class BootLoader : MonoBehaviour
    {
        [SerializeField] private ViewStorage _viewStorage;
        [SerializeField] private ControllerStorage _controllerStorage;
        [SerializeField] private UIFactory _uiFactory;
        [SerializeField] private bool _runTests = true;

        private void Start()
        {
            _viewStorage.Initialize();
            _controllerStorage.Initialize();
            _uiFactory.Initialize(_viewStorage, _controllerStorage);

            UIController uiController = _controllerStorage.GetValue<UIController>();
            uiController.Show<UIMainScreen>();

            if(_runTests)
            {
                TestRunner testRunner = new TestRunner();
                testRunner.Run();
            }
        }
    }
}
