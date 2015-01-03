using BarManager.Abstract.ViewModel;
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

        public RegisterViewModel(IAccessViewModel accessViewModel)
        {
            _accessViewModel = accessViewModel;
        }
    }
}
