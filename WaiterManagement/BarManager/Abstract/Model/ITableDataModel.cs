using System.Collections.Generic;
using BarManager.ManagerDataAccessWCFService;
namespace BarManager.Abstract.Model
{
    public interface ITableDataModel
    {

        Table AddTable(int number, string Description);

        bool EditTable(Table _table, int checkedNumber, string Description);

        IList<Table> GetAllTables();

        bool DeleteTable(int p);
    }
}