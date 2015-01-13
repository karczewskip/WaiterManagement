using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BarManager.Abstract
{
    public interface IManagerDataAccess : ICommunicationObject, BarManager.ManagerDataAccessWCFService.IManagerDataAccessWCFService
    {
    }
}
