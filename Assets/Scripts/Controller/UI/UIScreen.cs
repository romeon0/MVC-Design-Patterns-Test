using UnityEngine;

namespace UnityGame.MVC
{
    public class UIScreen : MonoBehaviour, IScreen
    {
        [SerializeField] private GameObject _root;
        protected IUIController _uiController;
        protected bool _initialized;
        private bool _shown;

        public void Initialize(IUIController controller)
        {
            _uiController = controller;

            InitializeInternal();

            _initialized = true;
        }

        protected virtual void InitializeInternal()
        {

        }

        public void Show()
        {
            gameObject.transform.SetAsLastSibling();
            _root.SetActive(true);

            if (!_shown)
            {
                ShowInternal();
            }
            _shown = true;
        }

        protected virtual void ShowInternal()
        {

        }

        public void Hide()
        {
            gameObject.transform.SetAsFirstSibling();
            _root.SetActive(false);

            if (_shown)
            {
                HideInternal();
            }
            _shown = false;
        }

        protected virtual void HideInternal()
        {

        }
    }
}

