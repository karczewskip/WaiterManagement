using BarManager.Abstract;
using BarManager.Abstract.Model;
using BarManager.ManagerDataAccessWCFService;
namespace BarManager.Model
{
    public class CredentialsDataModel : ICredentialDataModel
    {
        private readonly IManagerDataAccess _managerDataAccess;

        public CredentialsDataModel(IManagerDataAccess managerDataAccess)
        {
            _managerDataAccess = managerDataAccess;
            _managerDataAccess.SetCredentialDataModel(this);
            UserContext = null;
        }

        public UserContext UserContext { get; set; }

        public bool IsLogged()
        {
            return UserContext != null;
        }

        public void LogIn(string login, string password)
        {
            UserContext = _managerDataAccess.LogIn(login, ClassLib.DataStructures.HashClass.CreateFirstHash(password, login));
        }

        public void Register(string firstName, string lastName, string login, string password)
        {
            _managerDataAccess.AddManager(firstName, lastName, login, ClassLib.DataStructures.HashClass.CreateFirstHash(password, login));
            LogIn(login, password);
        }
    }
}