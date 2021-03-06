﻿namespace WebUI.Infrastructure.Abstract
{
    public interface IAuthProvider
    {
        bool IsLogged { get; }

        bool Authenticate(string username, string password);

        int GetClientId();
    }
}