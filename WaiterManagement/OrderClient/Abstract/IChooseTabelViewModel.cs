namespace OrderClient.Abstract
{
    internal interface IChooseTabelViewModel
    {
        void InitializeData();
        void SetMainWindowReference(IMainWindowViewModel mainWindowViewModel);
    }
}