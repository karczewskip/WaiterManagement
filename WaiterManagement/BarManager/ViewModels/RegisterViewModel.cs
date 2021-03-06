﻿using BarManager.Abstract.Model;
using BarManager.Abstract.ViewModel;
using BarManager.Messaging;
using BarManager.Views;

namespace BarManager.ViewModels
{
    internal class RegisterViewModel : IRegisterViewModel
    {
        private readonly ICredentialDataModel _credentialDataModel;
        private IAccessViewModel _accessViewModel;

        public RegisterViewModel( ICredentialDataModel credentialDataModel)
        {
            _credentialDataModel = credentialDataModel;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }

        public void SetParentWindow(IAccessViewModel accessViewModel)
        {
            _accessViewModel = accessViewModel;
        }

        public void Register(RegisterView view)
        {
            if (view.PasswordB.Password != view.ConfirmedPasswordB.Password)
            {
                Message.Show("Not confirmed password");
                return;
            }

            _credentialDataModel.Register(FirstName, LastName, UserName, view.PasswordB.Password);
            _accessViewModel.LogIn();
        }
    }
}