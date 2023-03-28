namespace UnityGame.MVC
{
    public interface IUIController : IController
    {
        public void Show<T>() where T : IScreen;
        public void Hide<T>() where T : IScreen;
    }
}

