using Caliburn.Micro;
using ORMWPFUserInterface.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ORMWPFUserInterface
{
    // Note:Used to set up caliber
    public class Bootstrapper:BootstrapperBase
    {
        private SimpleContainer _simpleContainer= new SimpleContainer();//Note: Instantiation like this will not happen often from now on.
                                                                        //      simpleContiner is a injection system.
        
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            _simpleContainer.Instance(_simpleContainer);//Note:Initialize the container
            _simpleContainer
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>();//Note:singleton means one instance for the life of application

            GetType().Assembly.GetTypes()// Note:Get all the types in current assembly
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel")) //Note: add some filter
                .ToList()
                .ForEach(viewModelType => _simpleContainer.RegisterPerRequest(
                    viewModelType, viewModelType.ToString(), viewModelType));


        }
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewForAsync<ShellViewModel>();//Note: lanuch shell view model as our base view
        }

        protected override object GetInstance(Type service, string key)
        {
            return _simpleContainer.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service) 
        { 
            return _simpleContainer.GetAllInstances(service); 
        }

        protected override void BuildUp(object instance)
        {
            _simpleContainer.BuildUp(instance);
        }
    }
}
