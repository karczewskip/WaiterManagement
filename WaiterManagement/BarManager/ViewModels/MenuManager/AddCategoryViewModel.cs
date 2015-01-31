using BarManager.Abstract;
using BarManager.Messaging;
using System.ComponentModel;
using System.Linq;
using BarManager.Abstract.Model;
using BarManager.Abstract.ViewModel;

namespace BarManager.ViewModels
{
    /// <summary>
    /// Klasa odpowiedzialna za dodawanie kategorii
    /// </summary>
    public class AddCategoryViewModel : IAddCategoryViewModel , INotifyPropertyChanged
    {
        private readonly IMenuDataModel _menuDataModel;
        private readonly IMenuManagerViewModel _menuManagerViewModel;

        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }

        public AddCategoryViewModel(IMenuDataModel menuDataModel, IMenuManagerViewModel menuManagerViewModel)
        {
            _menuDataModel = menuDataModel;
            _menuManagerViewModel = menuManagerViewModel;
        }

        public void AddCategory()
        {
            if (string.IsNullOrEmpty(CategoryName) || string.IsNullOrEmpty(CategoryDescription))
            {
                Message.Show("Some Fields are empty");
                return;
            }

            if( _menuManagerViewModel.Categories.Any(cat => cat.Name.Equals(CategoryName)))
            {
                Message.Show("There is category named: " + CategoryName);
                return;
            }

            var addingCategory = _menuDataModel.AddCategoryItem(CategoryName, CategoryDescription);
            if (addingCategory != null)
            {
                _menuManagerViewModel.AddCategoryToViewModel(addingCategory);
                _menuManagerViewModel.CloseDialogs();
                return;
            }

            Message.Show("Failed");

            return;
        }


        public void Clear()
        {
            CategoryName = "";
            CategoryDescription = "";

            if (null != this.PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("CategoryName"));
                PropertyChanged(this, new PropertyChangedEventArgs("CategoryDescription"));
                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
