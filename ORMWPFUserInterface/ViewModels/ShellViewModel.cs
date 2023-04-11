using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMWPFUserInterface.ViewModels
{
    public class ShellViewModel:Conductor<object>
    {
        private LoginViewModel _loginVM;
        public  ShellViewModel(LoginViewModel loginVM)
        {

            _loginVM = loginVM;// Note:Initialized in Bootstrapper by depenancy injection
            ActivateItemAsync(loginVM);//  display by Caliburn.Micro
        }

    }
}
