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
    /// <summary>
    /// Klasa odpowiedzialna za przetwarzanie danych stolików
    /// </summary>
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

        public bool DeleteSelectedItem(out string error)
        {
            if (SelectedTable == null)
            {
                error = "No Item Is Selected";
                return false;
            }
            else
            {
                if (DataModel.DeleteTable(SelectedTable.Id))
                {
                    ListOfTables.Remove(SelectedTable);
                    error = "";
                    return false;
                }
                else
                {
                    error = "Failed";
                    return false;
                }
            }
        }
    }
}
