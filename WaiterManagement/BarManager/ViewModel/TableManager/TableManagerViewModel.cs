using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarManager.Abstract;
using ClassLib.DbDataStructures;
using System.Collections.ObjectModel;
using System.Windows;

namespace BarManager.ViewModel
{
    public class TableManagerViewModel : ITableManagerViewModel
    {
        private IBarDataModel DataModel;

        public Table SelectedTable { get; set; }

        public IList<Table> ListOfTables { get; set; }

        public TableManagerViewModel(IBarDataModel dataModel)
        {
            DataModel = dataModel;

            ListOfTables = new ObservableCollection<Table>();

            InitializeData();
        }

        private void InitializeData()
        {
            ListOfTables = new ObservableCollection<Table>(DataModel.GetAllTables());
        }

        public void DeleteSelectedItem()
        {
            if (SelectedTable == null)
                MessageBox.Show("No Item Is Sellected");
            else
            {
                if (DataModel.DeleteTable(SelectedTable.Id))
                    ListOfTables.Remove(SelectedTable);
                else
                    MessageBox.Show("Failed");
            }
        }
    }
}
