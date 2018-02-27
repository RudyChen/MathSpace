using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathSpace.ViewModel
{
   public class LoginViewModel: ViewModelBase
    {
        private RelayCommand loginCommand;

        public RelayCommand LoginCommand
        {
            get { return loginCommand; }
            set { Set(ref loginCommand, value); }
        }

        public bool IsLoginSucceed { get; internal set; }

        public LoginViewModel()
        {
            loginCommand = new RelayCommand(LoginUser);
            IsLoginSucceed = false;
        }

        private void LoginUser()
        {

            IsLoginSucceed = true;
        }
    }
}
