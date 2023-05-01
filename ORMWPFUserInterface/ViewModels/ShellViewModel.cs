using Caliburn.Micro;
using ORMWPFUI.EventModels;
using ORMWPFUI.Library.Model;
using ORMWPFUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ORMWPFUserInterface.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private LoginViewModel _loginVM;
        private IEventAggregator _events;
        private SalesViewModel _salesVM;
        private ILoggedInUserModel _user;

        public  ShellViewModel(LoginViewModel loginVM,IEventAggregator events,SalesViewModel salesViewModel,ILoggedInUserModel user)
        {
            _salesVM= salesViewModel;
            _loginVM = loginVM;// Note:Initialized in Bootstrapper by depenancy injection
            _user = user;
            ActivateItemAsync(IoC.Get<LoginViewModel>());//  display by Caliburn.Micro
            _events = events;
            _events.Subscribe(this);// This class subscribes this events.

        }
        public bool IsAccountVisible
        {
            get
            {
                var flag = false;
                if (!string.IsNullOrWhiteSpace(_user.Token))
                {
                    flag = true;
                }
                return flag;
            }
        }

        public Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            NotifyOfPropertyChange(() => IsAccountVisible);
            return ActivateItemAsync(_salesVM, cancellationToken);
        }

        public void ExitApplication()
        {
           TryCloseAsync();
        }

        public void Logout()
        {
            _user.LogOffUser();
            NotifyOfPropertyChange(()=> IsAccountVisible);
            ActivateItemAsync(IoC.Get<LoginViewModel>());//  display by Caliburn.Micro
        }
    }
}
