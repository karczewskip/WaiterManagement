using BarManager.ManagerDataAccessWCFService;
namespace BarManager.Abstract.Model
{
    public interface ICredentialDataModel
    {
        bool IsLogged();

        void LogIn(string login, string password);

        void Register(string firstName, string lastName, string login, string password);

        UserContext UserContext { get; set; }
    }
}