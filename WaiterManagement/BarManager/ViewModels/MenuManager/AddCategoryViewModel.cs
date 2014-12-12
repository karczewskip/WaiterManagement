using BarManager.Abstract;
using System.ComponentModel;
using System.Linq;

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

        public bool AddCategory(out string error)
        {
            if (string.IsNullOrEmpty(CategoryName) || string.IsNullOrEmpty(CategoryDescription))
            {
                error = "Some Fields are empty";
                return false;
            }

            if( MenuManagerViewModel.Categories.Any(cat => cat.Name.Equals(CategoryName)))
            {
                error = "There is category named: " + CategoryName;
                return false;
            }

            var addingCategory = DataModel.AddCategoryItem(CategoryName, CategoryDescription);
            if (addingCategory != null)
            {
                MenuManagerViewModel.AddCategory(addingCategory);
                error = "";
                return true;
            }

            error = "Failed";

            return false;
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
