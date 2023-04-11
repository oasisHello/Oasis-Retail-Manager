using Caliburn.Micro;
using ORMWPFUserInterface.Heplers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ORMWPFUserInterface.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _userName;
        private string _password;
        private IAPIHelper _anAPIHelper;
        public LoginViewModel(IAPIHelper aAPIHelper)
        {
            _anAPIHelper = aAPIHelper;
            
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public bool CanLogIn
        {
            get
            {
                bool result = false;
                if (_userName?.Length > 0 || _password?.Length > 0)
                {
                    result = true;
                }
                return result;

            }
        }
        public async Task LogIn()
        {
            try
            {
                var result = await _anAPIHelper.Authenticate(UserName, Password);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



    }
}
