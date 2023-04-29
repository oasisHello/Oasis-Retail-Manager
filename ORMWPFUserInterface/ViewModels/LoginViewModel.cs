using Caliburn.Micro;
using ORMWPFUI.EventModels;
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
        private string _userName="gongstats@orm.com";
        private string _password="Pwd12345.";
        private IAPIHelper _anAPIHelper;
        private IEventAggregator _events;
        public LoginViewModel(IAPIHelper aAPIHelper,IEventAggregator events)
        {
            _anAPIHelper = aAPIHelper;
            _events = events;
            
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
                await _anAPIHelper.GetLoggedInUserInfo(result.Access_Token);
                await _events.PublishOnUIThreadAsync(new LogOnEvent());// I'm sure this event will be listened by other UI,
                                                // this way would not cause any cross thread issue.
                       
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }



    }
}
