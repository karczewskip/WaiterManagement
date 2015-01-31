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
        private IBarDataModel DataModel;
        private IMenuManagerViewModel MenuManagerViewModel;

        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }

        public AddCategoryViewModel(IBarDataModel dataModel, IMenuManagerViewModel menuManagerViewModel)
        {
            DataModel = dataModel;
            MenuManagerViewModel = menuManagerViewModel;
        }

        public void AddCategory()
        {
            if (string.IsNullOrEmpty(CategoryName) || string.IsNullOrEmpty(CategoryDescription))
            {
                Message.Show("Some Fields are empty");
                return;
            }

            if( MenuManagerViewModel.Categories.Any(cat => cat.Name.Equals(CategoryName)))
            {
                Message.Show("There is category named: " + CategoryName);
                return;
            }

            var addingCategory = DataModel.AddCategoryItem(CategoryName, CategoryDescription);
            if (addingCategory != null)
            {
                MenuManagerViewModel.AddCategoryToViewModel(addingCategory);
                MenuManagerViewModel.CloseDialogs();
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
