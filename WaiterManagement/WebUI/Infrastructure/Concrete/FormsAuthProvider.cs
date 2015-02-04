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

        private UserContext _userContext;
        private System.Web.HttpCookie _authenticationCookie;

        public string UserName
        {
            get
            {
                var ticket = FormsAuthentication.Decrypt(_authenticationCookie.Value);

                return ticket.UserData;
            }
        }

        public bool IsLogged
        {
            get
            {
                _authenticationCookie = HttpContext.Current.Request.Cookies.Get(FormsAuthentication.FormsCookieName);

                if (_authenticationCookie == null)
                    return false;

                return !string.IsNullOrEmpty(_authenticationCookie.Value);
            }
        }

        public FormsAuthProvider(IClientDataAccess clientDataAccess)
        {
            _clientDataAccess = clientDataAccess;
        }

        public bool Authenticate(string username, string password)
        {
            _userContext = _clientDataAccess.LogIn(username, HashClass.CreateFirstHash(password, username));

            if (_userContext != null)
            {
                const int timeout = 60;
                var ticket = new FormsAuthenticationTicket(username, true, timeout);
                var encrypted = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted)
                {
                    Expires = System.DateTime.Now.AddMinutes(timeout)
                };

                HttpContext.Current.Response.Cookies.Add(cookie);
                FormsAuthentication.SetAuthCookie(username,false);

                _authenticationCookie = FormsAuthentication.GetAuthCookie(FormsAuthentication.FormsCookieName, false);
                
                return true;
            }

            return false;
        }
    }
}