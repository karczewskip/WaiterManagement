namespace WebUI.Infrastructure.Abstract
{
    public interface IAuthProvider
    {
        string UserName { get; }

        bool IsLogged { get; }

        bool Authenticate(string username, string password); 
    }
}