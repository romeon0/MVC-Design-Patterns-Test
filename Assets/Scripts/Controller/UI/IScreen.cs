namespace UnityGame.MVC
{
    public interface IScreen
    {
        public void Initialize(IUIController uiController);
        public void Show();
        public void Hide();
    }
}

