using System;
using System.Web;
using System.Web.Security;
using ClassLib.DataStructures;
using WebUI.Infrastructure.Abstract;

namespace WebUI.Infrastructure.Concrete
{
    public class FormsAuthProvider : IAuthProvider
    {
        private readonly IClientDataAccess _clientDataAccess;

        private UserContext _userContext;
        private HttpCookie _authenticationCookie;

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
            _userContext = _clientDataAccess.LogIn(username, ClassLib.DataStructures.HashClass.CreateFirstHash(password, username));

            if (_userContext != null)
            {
                const int timeout = 60;
                var ticket = new FormsAuthenticationTicket(
                    1,
                    username, 
                    DateTime.Now,
                    DateTime.Now.AddMinutes(timeout), 
                    true,
                    _userContext.Id.ToString()
                    );
                var encrypted = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);

                HttpContext.Current.Response.Cookies.Add(cookie);

                _authenticationCookie = HttpContext.Current.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
                
                return true;
            }

            return false;
        }


        public int GetClientId()
        {
            _authenticationCookie = HttpContext.Current.Request.Cookies.Get(FormsAuthentication.FormsCookieName);

            var decrypted = FormsAuthentication.Decrypt(_authenticationCookie.Value);

            if (decrypted == null)
                return -1;

            return int.Parse(decrypted.UserData);
        }
    }
}