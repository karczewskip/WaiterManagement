using System;
using System.Collections.Generic;
using System.Linq;
using BarManager.Abstract;
using BarManager.Abstract.Model;
using BarManager.ManagerDataAccessWCFService;

namespace BarManager.Model
{
    public class TableDataModel : ITableDataModel
    {
        private readonly IManagerDataAccess _managerDataAccess;

        public TableDataModel(IManagerDataAccess managerDataAccess)
        {
            _managerDataAccess = managerDataAccess;
        }

        public IList<Table> GetAllTables()
        {
            try
            {
                return _managerDataAccess.GetTables().ToList();
            }
            catch
            {
                throw new Exception("Exception from DB");
            }
        }

        public bool DeleteTable(int id)
        {
            bool result;
            try
            {
                result = _managerDataAccess.RemoveTable(id);
            }
            catch
            {
                throw new Exception("Exception from DB");
            }

            return result;
        }

        public Table AddTable(int number, string tableDescription)
        {
            Table addingTable;
            try
            {
                addingTable = _managerDataAccess.AddTable(number, tableDescription);
            }
            catch
            {
                throw new Exception("Exception from DB");
            }

            return addingTable;
        }

        public bool EditTable(Table table, int number, string tableDescription)
        {
            bool result;

            var oldNumber = table.Number;
            var oldDescription = table.Description;

            table.Number = number;
            table.Description = tableDescription;

            try
            {
                result = _managerDataAccess.EditTable(table);
            }
            catch
            {
                table.Number = oldNumber;
                table.Description = oldDescription;

                throw new Exception("Exception from DB");
            }

            if (result) return true;

            table.Number = oldNumber;
            table.Description = oldDescription;

            return false;
        }
    }
}