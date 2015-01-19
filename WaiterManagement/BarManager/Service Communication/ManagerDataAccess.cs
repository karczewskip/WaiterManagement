using BarManager.Abstract;
using BarManager.ManagerDataAccessWCFService;

namespace BarManager.Service_Communication
{
    class ManagerDataAccess : ManagerDataAccessWCFServiceClient, IManagerDataAccess
    {
    }
}
