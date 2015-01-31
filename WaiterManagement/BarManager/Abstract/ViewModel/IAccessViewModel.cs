namespace BarManager.Abstract.ViewModel
{
    public interface IAccessViewModel
    {
        void ShowLogIn();
        void LogIn();
        void SetMainWindow(IMainWindowViewModel mainWindowViewModel);
    }
}