using BarManager.ManagerDataAccessWCFService;

namespace BarManager.Abstract.ViewModel
{
    public interface IEditMenuItemViewModel
    {
        void RefreshItem(MenuItem menuItem);

        void EditMenuItem();
    }
}
