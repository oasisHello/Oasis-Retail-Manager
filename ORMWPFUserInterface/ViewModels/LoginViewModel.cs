using Caliburn.Micro;
using ORMWPFUI.Library.API;
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


        public bool IsErrorVisible
        {
            get 
            {
                bool result = false;
                if(ErrorMessage?.Length > 0)
                {
                    result = true;
                }
                return result;
            }
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set 
            { 
                _errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
                NotifyOfPropertyChange(() => IsErrorVisible);
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
                ErrorMessage = "";
                var result = await _anAPIHelper.Authenticate(UserName, Password);

                //Capture more detailed information of the user.
                _anAPIHelper.GetLoggedInUserInfo(result.Access_Token);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }



    }
}
