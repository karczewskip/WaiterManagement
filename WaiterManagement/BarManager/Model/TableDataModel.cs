using BarManager.Abstract;
namespace BarManager.Model
{
    public class TableDataModel
    {
        private readonly IManagerDataAccess _managerDataAccess;

        public TableDataModel(IManagerDataAccess managerDataAccess)
        {
            _managerDataAccess = managerDataAccess;
        }


    }
}