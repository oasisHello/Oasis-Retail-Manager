using Caliburn.Micro;
using ORMWPFUI.EventModels;
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
        private SimpleContainer _container;

        public  ShellViewModel(LoginViewModel loginVM,IEventAggregator events,SalesViewModel salesViewModel,
            SimpleContainer simpleContainer)
        {
            _container = simpleContainer;
            _salesVM= salesViewModel;
            _loginVM = loginVM;// Note:Initialized in Bootstrapper by depenancy injection
            ActivateItemAsync(_container.GetInstance<LoginViewModel>());//  display by Caliburn.Micro
            _events = events;
            _events.Subscribe(this);// This class subscribes this events.

        }

        public Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            return ActivateItemAsync(_salesVM, cancellationToken);
        }

    }
}
