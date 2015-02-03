using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ClassLib.DataStructures;
using DataAccess;
using WebUI.Infrastructure.Abstract;

namespace WebUI.Infrastructure.Concrete
{
    public class FormsAuthProvider : IAuthProvider
    {
        private readonly IClientDataAccess _clientDataAccess;
        private readonly IManagerDataAccess _managerDataAccess;

        private UserContext _userContext;
        private System.Web.HttpCookie _authenticationCookie;

        public string UserName
        {
            get { return HttpContext.Current.User.Identity.Name; }
        }

        public bool IsLogged
        {
            get { return !string.IsNullOrEmpty(_authenticationCookie.Value); }
        }

        public FormsAuthProvider(IClientDataAccess clientDataAccess, IManagerDataAccess managerDataAccess)
        {
            _clientDataAccess = clientDataAccess;
            _managerDataAccess = managerDataAccess;

            _authenticationCookie = FormsAuthentication.GetAuthCookie(FormsAuthentication.FormsCookieName, false);
        }

        public bool Authenticate(string username, string password)
        {
            _userContext = _managerDataAccess.LogIn(username, HashClass.CreateFirstHash(password, username));

            if (_userContext != null)
            {
                FormsAuthentication.SetAuthCookie(username,false);
                _authenticationCookie = FormsAuthentication.GetAuthCookie(FormsAuthentication.FormsCookieName, false);
                
                return true;
            }

            return false;
        }
    }
}