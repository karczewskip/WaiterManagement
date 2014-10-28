using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarManager.Abstract;
using System.Windows;

namespace BarManager.ViewModel
{
    public class AddCategoryViewModel : IAddCategoryViewModel
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

            if( MenuManagerViewModel.ListOfCategories.Any(cat => cat.Name.Equals(CategoryName)))
            {
                error = "There is category named: " + CategoryName;
                return false;
            }

            var AddingCategory = DataModel.AddCategoryItem(CategoryName, CategoryDescription);
            if (AddingCategory != null)
            {
                MenuManagerViewModel.ListOfCategories.Add(AddingCategory);
                error = "";
                return true;
            }

            error = "Failed";

            return false;
        }
    }
}
