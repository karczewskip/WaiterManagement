using BarManager.Abstract;
using BarManager.Abstract.ViewModel;
using BarManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarManager.ViewModels
{
    class RegisterViewModel : IRegisterViewModel
    {
        private IAccessViewModel _accessViewModel;
        private IBarDataModel _barDataModel;

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public RegisterViewModel(IAccessViewModel accessViewModel, IBarDataModel barDataModel)
        {
            _accessViewModel = accessViewModel;
            _barDataModel = barDataModel;
        }

        public void Register(RegisterView view)
        {
            //MessageBox.Show(view.PasswordB.Password);
            //MessageBox.Show("User Name == " + _userName + ", password == " + HashClass.CreateFirstHash(view.PasswordB.Password, _userName));
            if(view.PasswordB.Password != view.PasswordB.Password)
            {
                Messaging.Message.Show("Not confirmed password");
                return;
            }

            _barDataModel.Register(_firstName, _lastName, UserName, view.PasswordB.Password);
            _accessViewModel.LogIn();
        }

    }
}
